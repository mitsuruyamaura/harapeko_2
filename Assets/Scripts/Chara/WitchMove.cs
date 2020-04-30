using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WitchMove : MonoBehaviour {

    [Header("キャラNo")]
    public int charaNum;
    [Header("表示キャラ")]
    public GameObject charaObj;
    [Header("移動速度")]
    public float moveSpeed;
    [Header("ジャンプ力")]
    public float jumpPower;
    [Header("チャージ攻撃時の消費ライフ量")]
    public int chargePoint;
    [Header("攻撃力")]
    public static int attackPower;
    [Header("ライフ減少速度")]
    public float lifeLoss;
    [Header("スコアボーナス")]
    public float scoreBonus;

    [Header("Linecast用 地面判定レイヤー")]
    public LayerMask groundLayer;
    [Header("Linecast 空中床判定レイヤー")]
    public LayerMask bottomLayer;

    [Header("パワーアップ時の弾のプレファブ")]
    public GameObject powerfulStar;
    [Header("チャージ攻撃時の弾のプレファブ")]
    public GameObject attackStar;
    [Header("通常攻撃の弾のプレファブ")]
    public GameObject candyBullet;
    [Header("攻撃ヒットエフェクト用のプレファブ")]
    public GameObject hitEffect;

    public GameObject invisibleEffect;
    private float blinkTime;
    private RectTransform rt;

    public enum WitchState{              //  キャラクターのステート管理用
        NORMAL,
        FLY,
        POWERFUL,
        DOWN
    }
    public WitchState witchState;    //  現在のキャラクターのステートを入れる
    
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D myCol;
    ParticleSystem particle;
    Life life;

    AudioSource sound1;                 //SE1-5
    AudioSource sound2;
    AudioSource sound3;
    AudioSource sound4;
    AudioSource sound5;                 //  パワーアップ中のジングル

    int soundPlay = 0;                  //  BGM管理用
    float witchStateTimer =0;           //  witchStateの変更の残り時間を示す。0になったらwitchState=0にする
    float chargeCount = 0f;             //  チャージ攻撃カウント用
   
    bool isGrounded;                    //  着地判定。trueで着地中＝ジャンプできる
    bool isBottomed;                    //  一番下の足場にいるかどうかの判定用（画面外への落下防止）
    bool isDamage;                      //  ダメージを受けたかどうかのフラグ。trueで被ダメージ中
    bool isAttacked;                    //  攻撃中フラグ

    public void SetUp(Character.CharaData chara) {
        charaNum = chara.charaNum;
        moveSpeed = chara.moveSpeed;
        jumpPower = chara.jumpPower;
        chargePoint = chara.chargePoint;
        attackPower = chara.attackPower;
        lifeLoss = chara.lifeLoss;
        scoreBonus = chara.scoreBonus;

        life = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
        rt = life.GetComponent<RectTransform>();
        life.GetLifeLossSpeed(lifeLoss);
        GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<Score>().GetScoreBonus(scoreBonus);

        //  各コンポーネントを取得
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myCol = GetComponent<CapsuleCollider2D>();
        particle = GetComponent<ParticleSystem>();
        
        //  SE用の配列の宣言
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        sound2 = audioSources[1];
        sound3 = audioSources[2];
        sound4 = audioSources[3];
        sound5 = audioSources[4];
    }

    /// <summary>
    /// 無敵時間時のキャラ点滅処理
    /// </summary>
    /// <returns></returns>
    private void Blink() {
        invisibleEffect.SetActive(!invisibleEffect.activeSelf);
    }

    //  キーダウンなどの処理はUpdateを使う     
    void Update(){    
        if (Mathf.Approximately(Time.timeScale, 0f)){
            //  Time.timeScaleが0の場合はアップデートを止めて操作を無効にしておく
            return;
        }
        if(life.gameState == Life.GameState.GAME_OVER && witchState != WitchState.DOWN) {
            Dead();
            return;
        }
        if (isDamage) {
            //  ダメージフラグがtrueであればキャラ点滅させる(2秒)
            blinkTime += Time.deltaTime;       
            if(blinkTime >= 0.15f) {
                Blink();
                blinkTime = 0;
            }
            //float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            //  アルファを制御して点滅しているように見せる
            //sRenderer.color = new Color(1f, 1f, 1f, level);
        }
        //  ゲームオーバーでないなら実行する
        if (life.gameState != Life.GameState.GAME_OVER) {     
            //  ジャンプ処理
            Jump();

            if (!isAttacked) {
                //  攻撃処理
                //  isAttakedでなければキー操作で弾を発射する
                Attack();
            }
        }
    }

    private void Jump() {
        //  fly状態かどうか判定。fly状態なら無限ジャンプ
        //  この状態からはnomalには戻らない。そのため、powerflで上書きされるまで有効
        if (witchState == WitchState.FLY) {
            //  制限時間をカウント開始
            witchStateTimer += Time.deltaTime;
            if (witchStateTimer > 10.0f) {
                UndoState();
            }
            //  ジャンプ連打で空中歩行。落下時はホバリング可能。
            if (CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetButtonDown("Jump")) {
                //  AddForceにて上方向へ力を加える
                rb.AddForce(new Vector2(1, 1) * jumpPower / 2);
            }
        } else {   //  WitchStateがfly以外の場合。通常のジャンプ処理          
                   //  Linecastでキャラの足元に地面があるか判定①　全般用
            isGrounded = Physics2D.Linecast(
            transform.position + transform.up * 1f,
            transform.position - transform.up * 0.3f,
            groundLayer);

            //  Linecatでキャラの足元に地面があるか判定②　一番下の足場用
            isBottomed = Physics2D.Linecast(
            transform.position + transform.up * 0f,
            transform.position - transform.up * 0.3f,
            bottomLayer);

            //  落下していなくてBottom以外の足場でスルー（矢印の下かSキーのいずれか）が押されている間にジャンプが押されたら
            if (CrossPlatformInputManager.GetButtonDown("Through") || Input.GetButtonDown("Through")) {
                if (isGrounded) {
                    //  着地判定をさせず、ジャンプをさせない
                    isGrounded = false;
                    //  トリガーを入れて床をすり抜ける。OnTriggerExit2Dで戻す
                    myCol.isTrigger = true;
                    //  isTriggerを時間差で戻す処理を呼び出す
                    StartCoroutine(LandingGround());
                }
            }
            //  ジャンプ（スペースキーか矢印の上）を押したら
            if (CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetButtonDown("Jump")) {
                //  BottomかGroundに着地していたとき
                if (isGrounded || isBottomed) {//&& isJumping == false ){
                                               //  着地判定をfalse
                    isGrounded = false;
                    isBottomed = false;
                    //  AddForceにて上方向へ力を加える
                    rb.AddForce(new Vector2(1, 1) * jumpPower);
                    //  SE
                    sound1.PlayOneShot(sound1.clip);
                }
            }
            //  上下への移動速度を取得
            //float velY = rigidbody2D.velocity.y;

            //  移動速度が0.1よりも大きければ上昇
            //isJumping = velY > 0.1f ? true : false;

            //  移動速度が0.1よりも小さければ下降
            //isFalling = velY < 0.1f ? true : false;                
        }
    }

    private void Attack() {
        // Powerful状態かどうか判定
        if (witchState == WitchState.POWERFUL) {
            //  制限時間をカウント開始
            witchStateTimer += Time.deltaTime;
            //　10秒経過したら
            if (witchStateTimer > 10.0f) {
                UndoState();
            }
            //  Power状態での攻撃。すべてチャージショット。なので常にparticle点滅。
            if (CrossPlatformInputManager.GetButtonUp("Action") || Input.GetButtonUp("Action")) {
                //　チャージ攻撃でオブジェクト貫通弾
                StartCoroutine(CreateBullet(powerfulStar, 1.0f));
            }
        } else {  //  witchStateがPowerful状態でないなら（通常かFly状態）
            if (CrossPlatformInputManager.GetButton("Action") || Input.GetButton("Action")) {
                //  チャージ攻撃カウント用
                chargeCount += Time.deltaTime;
            }
            if (CrossPlatformInputManager.GetButtonDown("Action") || Input.GetButtonDown("Action")) {
                //   チャージ状態を示すParticleとSEを鳴らす
                particle.Play();
                sound4.PlayOneShot(sound4.clip);
            }
            //　キーを離した時のカウントの内容で分岐
            if (CrossPlatformInputManager.GetButtonUp("Action") || Input.GetButtonUp("Action")) {
                if (chargeCount < 1.5f) {
                    //  チャージのカウントが足りない場合、通常攻撃
                    StartCoroutine(CreateBullet(candyBullet, 0.4f));

                    //  fly状態でなければ効果音ならす。割れ防止。
                    if (witchState != WitchState.FLY) {
                        sound2.PlayOneShot(sound2.clip);
                    }
                } else {
                    //　チャージ攻撃
                    StartCoroutine(CreateBullet(attackStar, 1.0f));

                    //  Lifeスクリプトのライフの減算メソッドを呼び出す
                    life.SubtractLife(chargePoint);
                    CheckLife();

                    //  fly状態でなければ効果音ならす。割れ防止。
                    if (witchState != WitchState.FLY) {
                        sound3.PlayOneShot(sound3.clip);
                    }
                }
                chargeCount = 0;
            }
            //  パーティクルが再生中＋キーが上がったらパーティクルの再生を止める
            if (particle.isPlaying && CrossPlatformInputManager.GetButtonUp("Action") || particle.isPlaying && Input.GetButtonUp("Action")) {
                particle.Stop();
            }
        }
    }

    /// <summary>
    /// 弾を生成
    /// </summary>
    /// <param name="bulletPrefab"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    private IEnumerator CreateBullet(GameObject bulletPrefab, float pos) {
        isAttacked = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.2f);
        
        // 向きに合わせて弾を生成
        Instantiate(bulletPrefab, transform.position + new Vector3(pos * -transform.localScale.x, pos, 0f), transform.rotation);      
        //if(!adMob.isAdmob) {
        yield return new WaitForSeconds(0.85f);
        //} else {
        // 広告を見た場合
        //yield return new WaitForSeconds(0.6f);
        //}
        isAttacked = false;
    }

    /// <summary>
    /// プレイヤーのライフ確認。チャージ攻撃でHpが減少するため
    /// </summary>
    private void CheckLife() {
        //　HpのsizeDelta(HPのバー)が0になったらDeadを呼出
        if(rt.sizeDelta.x < 0) {
            Dead();
        }
    }

    /// <summary>
    /// プレイヤーのパワーアップ処理。パワーアップアイテムを取得時
    /// </summary>
    private void PowerUp() {
        //  timerを0に戻す
        witchStateTimer = 0.0f;
        //  パワーアップ曲を止める(パワーアップ中なら曲がリセットされて再度リスタートする)
        sound5.Stop();
        //  BGMを元に戻す
        soundPlay = 0;
        //  BGM1を止めてパワーアップ状態のBGMに変える
        if(soundPlay == 0) {
            sound5.PlayOneShot(sound5.clip);
            soundPlay = 1;
        }
    }

    /// <summary>
    /// プレイヤーの状態をノーマルに戻す
    /// </summary>
    private void UndoState() {
        //  witchの状態を通常に戻す
        witchState = WitchState.NORMAL;
        //  timerを0に戻す
        witchStateTimer = 0.0f;
        //  BGMを元に戻す
        soundPlay = 0;
    }

    //  移動処理は一定間隔のFixedUpdateを使う
    //  （Rgidbody2Dなど、秒間に呼ばれる回数にバラつきのない方を使う）
    void FixedUpdate(){
        //  キャラクターを画面端でループさせる処理を常時行い確認する
        fieldLoop();

        if (life.gameState != Life.GameState.GAME_OVER) {
            // x = 左キー：-1、右キー：+1
            // x = PCとモバイル用のキー設定
            float x = 0;
            if(!DebugSwitch.isKeyFlg) {
                x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            } else {
                // PC用
                x = Input.GetAxisRaw("Horizontal");
            }
            //  左右を入力したら(x =0ではないなら)
            //  十字キーとバーチャルパッド共用。同時にはできない
            if (x != 0 ) {
                //  入力方向へ移動
                rb.velocity = new Vector3(x * moveSpeed, rb.velocity.y, 0);
                //localScale.xを-1すると画像が反転する
                Vector3 temp = transform.localScale;
                temp.x = -x;

                //  向きが変わるときに少数になるとキャラが縮んでしまうので
                //  それを正の数にしてちゃんと描画する値にしてから戻す
                //  数字が0よりも大きければすべて1にする
                if (temp.x > 0){
                    temp.x = 1;
                } else {     //  数字が0よりも小さければすべて-1にする
                    temp.x = -1;
                }
                transform.localScale = temp;  //  数値を戻す             
                //  歩くアニメを再生する
                anim.SetFloat("Run", 0.7f);
            } else {    //  左右の入力がなかったら横移動の速度を0にしてピタッと止まるようにする
                rb.velocity = new Vector3(0, rb.velocity.y, 0);
                //  アニメの再生を止めてアイドル状態にする
                anim.SetFloat("Run", 0.0f);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {    //　敵との当たり判定
        //  Enemyとぶつかった時、かつダメージフラグがfalseならノックバックさせる
        if(!isDamage && col.gameObject.tag == "Enemy") {
            KnockbackEffect();
        }
        //  State変更処理
        if(col.gameObject.tag == "Letter1") {
            //  witchの状態を1=Fly状態に切り替える
            witchState = WitchState.FLY;
            PowerUp();
        }
        if(col.gameObject.tag == "Letter2") {
            //  witchの状態を2=Powerful状態に切り替える
            witchState = WitchState.POWERFUL;
            PowerUp();
        }
        //  State変更処理、ここまで
        // 左右に動く床に乗ったら床を親にしてキャラを追従させる
        if (col.gameObject.tag == "MoveFloor") {
            transform.SetParent(col.transform);
        }
    }

    /// <summary>
    /// 左右に動く空中床から乗りたら親子関係を解除する処理
    /// </summary>
    /// <param name="col"></param>
    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "MoveFloor") {
            transform.SetParent(null);
        }
    }

    /// <summary>
    /// 空中床の通過が終了したらisTriggerをOFFにする
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerExit2D(Collider2D col){
        myCol.isTrigger = false;
    }

    /// <summary>
    /// プレイヤーのノックバック処理
    /// </summary>
    private void KnockbackEffect(){
        //  ダメージフラグをonにする
        isDamage = true;
        //  プレイヤーの位置を後ろに飛ばす
        float s = 50f * Time.deltaTime;
        transform.Translate(Vector3.up * s);
        //  プレイヤーのlocalScaleでどちらを向いているかを判定
        if (transform.localScale.x >= 0) {
            transform.Translate(Vector3.right * s);
        } else {
            transform.Translate(Vector3.left * s);
        }
        //  無敵時間を作る
        StartCoroutine(InvincibleTime());
    }

    /// <summary>
    /// 無敵時間処理。敵と接触した際に呼ばれる
    /// </summary>
    IEnumerator InvincibleTime(){
        //レイヤーをPlayerDamageに変更し、敵との当たり判定を無視する
        gameObject.layer = LayerMask.NameToLayer("PlayerDamege");
        yield return new WaitForSeconds(2.0f);
        isDamage = false;
        invisibleEffect.SetActive(true);
        //レイヤーをPlayerに戻す
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    /// <summary>
    /// 空中床を通過した後に着地状態に戻す
    /// </summary>
    /// <returns></returns>
    IEnumerator LandingGround(){
        yield return new WaitForSeconds(1.5f);
        isGrounded = true;
    }

    /// <summary>
    /// 画面の両端でループさせる処理
    //  万が一、画面下部に落下した場合も上から降ってくるようにする 
    /// </summary>
    private void fieldLoop(){
        if (rb.transform.position.x > 9){
            Vector3 pos = rb.transform.position;
            pos.x = pos.x - 18;
            rb.transform.position = pos;
        }
        if (rb.transform.position.x < -9){
            Vector3 pos = rb.transform.position;
            pos.x = pos.x + 18;
            rb.transform.position = pos;
        }
        if (rb.transform.position.y < -5){
            Vector3 pos = rb.transform.position;
            pos.y = pos.y + 11;
            rb.transform.position = pos;
        }
    }

    /// <summary>
    /// プレイヤー周りのゲームオーバー処理
    /// </summary>
    private void Dead(){
        if (witchState != WitchState.DOWN) {
            // 再生中のアニメを停止し、倒れるアニメを再生する
            anim.ResetTrigger("Attack");
            anim.SetFloat("Run",0.0f);
            anim.SetTrigger("Down");
            witchState = WitchState.DOWN;
        }
    }

    /// <summary>
    /// 敵に攻撃をヒットさせた際のエフェクト発生処理
    /// </summary>
    public void PlayHitEffect(Vector2 pos) {
        GameObject effect = Instantiate(hitEffect, pos, transform.rotation);
        Destroy(effect, 0.3f);
    }
}
