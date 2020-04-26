using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour {

    protected Rigidbody2D rb;
    protected Animator anim;

    protected int enemyPoint = 1;            //  敵を倒した数
    protected bool isDeleted;                //  ２回呼び出しを制御

    [Header("ボーナスステージ用")]
    public bool isBonus;
    [Header("通常攻撃での破壊不能")]
    public bool isBulletIndestroy;
    [Header("チャージ攻撃での破壊不能")]
    public bool isChargeIndestroy;
    [Header("Golem_R/Giant_R/Troll_Rはオン")]
    public bool isRightFace;
    [Header("移動速度")]
    public float moveSpeed = 0.3f;
    [Header("攻撃力")]
    public float attackPower = 200;　　//この数値分HP減少
    [Header("得点")]
    public float scorePoint = 1000;
    [Header("ライフ")]
    public int hp = 1;
    [Header("破壊されたときに生成するオブジェクトの指定")]
    public GameObject [] items;

    [Header("ジャンプ力")]
    public float jumpPower;

    [Header("各行動ごとの待機時間の最小値と最大値")]
    public RangeInteger[] m_ints;

    private int randomJumpWait;            //  ジャンプまでの待機時間
    private int randomFacingWait;          //  向きを変えるまでの待機時間
    protected int randomDashWait;
    private int randomMoveWait;
    private int randomAttackWait;
    private float jumpTimer;               //  ジャンプ用タイマー
    private float facingTimer;             //  方向転換用タイマー
    private int dashCount;
    private float attackTimer;
    private float moveTimer;

    private float currentMoveSpeed;
    private float currentAttackPower;

    public enum EnemyState {
        WALK,
        FACING,
        JUMP,
        DASH,
        MOVE,
        ATTACK,
        DOWN
    }

    public EnemyState enemyState = EnemyState.WALK;

    public EnemyType enemyType;

    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
        if(!isBonus) {
            anim = GetComponent<Animator>();
        }
        SetUp();
    }

    /// <summary>
    /// 各初期値を取得
    /// </summary>
    protected void SetUp() {
        currentMoveSpeed = moveSpeed;
        currentAttackPower = attackPower;
        //  各待機時間をランダムで設定。共に0ならUpdateでTimerしない
        randomJumpWait = Random.Range(m_ints[0].MinValue, m_ints[0].MaxValue);
        randomFacingWait = Random.Range(m_ints[1].MinValue, m_ints[1].MaxValue);
        randomDashWait = Random.Range(m_ints[2].MinValue, m_ints[2].MaxValue);
        randomMoveWait = Random.Range(m_ints[3].MinValue, m_ints[3].MaxValue);
        randomAttackWait = Random.Range(m_ints[4].MinValue, m_ints[4].MaxValue);

        if (enemyType == EnemyType.KELBEROS) {
            anim.speed = 3.5f;
        }
    }

    protected void Update() {
        if(!isBonus) {
            //  常時移動する。アニメの再生する
            rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, 0);
            if(moveSpeed == 0) {
                anim.SetBool("Idle", true);
            } else {
                anim.SetBool("Walk", true);
            }
        }

        if (enemyState != EnemyState.DOWN) {
            if (randomJumpWait != 0) {
                jumpTimer += Time.deltaTime;
            }
            if (randomFacingWait != 0) {
                facingTimer += Time.deltaTime;
            }
            if (randomAttackWait != 0) {
                attackTimer += Time.deltaTime;
            }
            if (randomMoveWait != 0) {
                moveTimer += Time.deltaTime;
            }
            if (jumpTimer > randomJumpWait && enemyState != EnemyState.JUMP) {
                Jump();
            }
            if (facingTimer > randomFacingWait && enemyState != EnemyState.FACING) {
                Facing();
            }
            if (attackTimer > randomAttackWait && enemyState != EnemyState.ATTACK) {
                Attack();
            }
            if (moveTimer > randomMoveWait && enemyState != EnemyState.MOVE) {
                Move();
            }
        }
    }

    //  画面両端時の移動処理
    protected void FixedUpdate() {
        fieldLoop();
    }

    //  プレイヤーの弾に接触した時の処理
    protected void OnTriggerEnter2D(Collider2D col) {
        if(!isDeleted){
            if(col.tag == "Charge" && !isChargeIndestroy) {
                //  ２回呼び出すのを制御
                isDeleted = true;
                Destroy();
            }       
            if(col.tag == "Bullet" && !isBulletIndestroy) {
                hp -= WitchMove.attackPower;
                Debug.Log(hp);
                GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchMove>().PlayHitEffect(transform.position);
                if(hp <= 0) {
                    isDeleted = true;
                    Destroy();
                }
            }
        }
    }

    /// <summary>
    /// 破壊処理とスコア加算処理
    /// </summary>
    protected void Destroy() {
        // 移動を止めて当たり判定をなくし、ダウンアニメ再生
        isBonus = true;
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        enemyState = EnemyState.DOWN;
        anim.SetBool("Dead", true);

        gameObject.layer = LayerMask.NameToLayer("DownEnemy");
        //　スコアコンポーネントを取得してポイントを渡す
        FindObjectOfType<Score>().AddPoint(scorePoint);
        Destroy(gameObject, 1.5f);
        //　ゲームクリアコンポーネントを取得してポイントを渡す
        FindObjectOfType<GameClearController>().AddDestroyPoint(enemyPoint);
        CreateItem();
    }

    /// <summary>
    /// アイテム生成処理
    /// </summary>
    protected void CreateItem() {
        //  アイテムをランダムで生成する
        int randomValue = Random.Range(0, items.Length);
        Instantiate(items[randomValue], transform.position, transform.rotation);
    }

    protected void OnCollisionEnter2D(Collision2D col) { 
        if(col.gameObject.tag == "Witch") {
            //  プレイヤーと接触時、攻撃力分ライフをを減少させるメソッドを呼び出し
            GameObject.FindGameObjectWithTag("HP").GetComponent<Life>().SubtractLife((int)attackPower);
        }
    }

    /// <summary>
    /// 画面の両端でループさせる処理
    /// </summary>
    protected void fieldLoop() {
        if(rb.transform.position.x > 9) {
            Vector3 rb = this.rb.transform.position;
            rb.x = rb.x - 18;
            this.rb.transform.position = rb;
        }
        if(rb.transform.position.x < -9) {
            Vector3 rb = this.rb.transform.position;
            rb.x = rb.x + 18;
            this.rb.transform.position = rb;
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump() {
        enemyState = EnemyState.JUMP;
        rb.velocity = new Vector3(-moveSpeed, jumpPower, 0);
        jumpTimer = 0f;
        randomJumpWait = Random.Range(m_ints[0].MinValue, m_ints[0].MaxValue);
        enemyState = EnemyState.WALK;
    }

    /// <summary>
    /// 方向転換処理
    /// </summary>
    private void Facing() {
        enemyState = EnemyState.FACING;
        //  向きを移動する方向に合わせる
        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
        moveSpeed *= -1;
        facingTimer = 0;
        randomFacingWait = Random.Range(m_ints[1].MinValue, m_ints[1].MaxValue);
        enemyState = EnemyState.WALK;
        if(randomDashWait != 0) {
            CheckDash();
        }
    }

    /// <summary>
    /// ダッシュ判定
    /// </summary>
    private void CheckDash() {
        dashCount++;
        if(dashCount >= randomDashWait) {
            Dash();            
        }
    }

    /// <summary>
    /// ダッシュ処理
    /// </summary>
    public void Dash() {
        enemyState = EnemyState.DASH;
        //  攻撃力を一時的に上げる
        attackPower *= 1.5f;
        //  向きを取得し、その方向で移動速度を調整
        moveSpeed *= 10f;
        anim.SetBool("Walk", false);
        anim.SetTrigger("Dash");
        StartCoroutine(DashMode());
    }

    /// <summary>
    /// ダッシュ後処理
    /// </summary>
    /// <returns></returns>
    IEnumerator DashMode() {
        yield return new WaitForSeconds(1.0f);
        //anim.SetBool("Walk", true);
        attackPower = currentAttackPower;
        OrientFace();
        dashCount = 0;
        enemyState = EnemyState.WALK;
        randomDashWait = Random.Range(m_ints[2].MinValue, m_ints[2].MaxValue);
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    private void Attack() {
        enemyState = EnemyState.ATTACK;
        attackPower = attackPower * 1.5f;
        anim.SetBool("Walk", false);
        moveSpeed = 0;
        anim.SetBool("Idle", false);
        anim.SetTrigger("Dash");
        StartCoroutine(AttackMode());
    }

    /// <summary>
    /// 攻撃後処理
    /// </summary>
    /// <param name="undoAttackPower"></param>
    /// <param name="undoMoveSpeed"></param>
    /// <returns></returns>
    IEnumerator AttackMode() {
        yield return new WaitForSeconds(1.5f);
        //anim.SetBool("Walk", true);
        attackPower = currentAttackPower;
        OrientFace();
        attackTimer = 0;
        enemyState = EnemyState.WALK;
        randomAttackWait = Random.Range(m_ints[4].MinValue, m_ints[4].MaxValue);
    }

    /// <summary>
    /// 進行方向と敵キャラの向きを合わせる処理
    /// </summary>
    public void OrientFace() {
        Vector3 temp = transform.localScale;
        if(isRightFace) {
            if(temp.x == 1) {
                moveSpeed = -currentMoveSpeed;
            } else {
                moveSpeed = currentMoveSpeed;
            }
        } else {
            if(temp.x == 1) {
                moveSpeed = currentMoveSpeed;
            } else {
                moveSpeed = -currentMoveSpeed;
            }
        }
        if (enemyType == EnemyType.KELBEROS) {
            anim.speed = 3.5f;
        }
    }

    /// <summary>
    /// 停止→移動処理
    /// </summary>
    private void Move() {
        enemyState = EnemyState.MOVE;
        Vector3 temp = transform.localScale;
        if(temp.x == 1) {
            moveSpeed = 1.0f;
        } else {
            moveSpeed = -1.0f;
        }
        anim.SetBool("Idle", false);
        anim.speed = 1.0f;
        anim.SetBool("Walk", true);
        StartCoroutine(StopMode());
    }

    /// <summary>
    /// 移動→停止処理
    /// </summary>
    /// <returns></returns>
    IEnumerator StopMode() {
        yield return new WaitForSeconds(1.8f);
        anim.SetBool("Walk", false);
        if (enemyType == EnemyType.KELBEROS) {
            anim.SetTrigger("Skill");
        } else {
            anim.SetBool("Idle", true);
        }
        OrientFace();
        moveTimer = 0;
        enemyState = EnemyState.WALK;
        randomMoveWait = Random.Range(m_ints[3].MinValue, m_ints[3].MaxValue);
    }
}

