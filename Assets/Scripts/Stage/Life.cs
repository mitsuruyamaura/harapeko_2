using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class Life : MonoBehaviour {

    [SerializeField, Header("リトライ用ポップアップ")]
    public RetryPopupManager retryPopup;
    [SerializeField, Header("時間経過によるライフの減少速度")]
    private float loss;

    public enum GameState {          //  WitchMoveでのキー操作やゲームオーバー判定にも利用する
        IN_PROGRESS,
        STAGE_CLEAR,
        GAME_OVER,
        EXCELENT
    }
    public GameState gameState;

    RectTransform rt;                //  UIなのでTransformではなくRectTransformを指定する
    Image image;                     //  HPが少なくなった時に色を変えるために取得する
    bool isHungry;                   //  空腹判定

    void Start (){
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    void Update() {
        // Time.timeScaleが0の場合はアップデートを止めておく
        if(Mathf.Approximately(Time.timeScale, 0f)) {
            return;
        }
        // 時間の経過に合わせてHP減少
        rt.sizeDelta -= new Vector2(loss, 0);

        // ライフが100以下になった時、はらぺこ状態になる。ゲージの色を変えて警告する。音は保留
        if(rt.sizeDelta.x < 100) {
            isHungry = true;
            // ゲージを赤くする
            image.color = new Color(1f, 0f, 0f, 1f);
        } else {
            // ライフが100以上に回復したらゲージを元の色に戻す
            image.color = new Color(1f, 1f, 1f, 1f);
        }
        // ライフが0以下になった時、ゲームオーバー状態でないならゲームオーバー処理
        if(gameState != GameState.GAME_OVER) {
            if(rt.sizeDelta.x <= 0) {
                GameOver();
            }
        }
        // ゲームオーバー状態でハイスコアの更新がないなら画面タップでタイトルに戻す
        if(gameState == GameState.GAME_OVER && !Score.isNewRecord) {
            //  ActionボタンかJumpボタンを押すと
            if(CrossPlatformInputManager.GetButtonDown("Action") || CrossPlatformInputManager.GetButtonDown("Jump") || Input.GetKeyDown("space")) {
                //SceneManager.LoadScene("Title");
            }
        }
    }

    /// <summary>
    /// 時間経過によるライフ減少量の取得処理
    /// キャラの種類に応じたライフ減少量を取得
    /// </summary>
    public void GetLifeLossSpeed(float lifeLoss) {
        loss = lifeLoss;
        Debug.Log(loss);
    }

    /// <summary>
    /// ライフ減少処理
    /// </summary>
    /// <param name="ap"></param>
    public void SubtractLife(int ap){
        if (gameState == GameState.IN_PROGRESS){
            // ゲーム進行中のみRectTransformのサイズを取得し、マイナスする
            rt.sizeDelta -= new Vector2(ap, 0);
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BgmManager>().DamageLifeSE();
        }
    }

    /// <summary>
    /// ライフ回復処理
    /// </summary>
    /// <param name="hp"></param>
    public void AddLife(int hp){
        if(gameState != GameState.GAME_OVER) {
            // ゲームオーバーでないなら、RectTrrancformのサイズを取得し、プラスする
            rt.sizeDelta += new Vector2(hp, 0);
            GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BgmManager>().RecoverLifeSE();
            // 最大値を超えたら最大値で上書きする
            if(rt.sizeDelta.x > 767f) {
                rt.sizeDelta = new Vector2(767f, 42f);
            }
        }
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    private void GameOver() {
        // ゲームオーバーにしてライフ減少を止める。
        gameState = GameState.GAME_OVER;
        loss = 0.0f;
        // ゲームオーバーリザルト表示し、ゲームオーバーBGMを鳴らす
        GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ResultDisplayManager>().DisplayGameOverText();
        GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BgmManager>().StopBgm();
        // ローカルにハイスコアを保存
        //GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<Score>().Save();
        // StageManagerスクリプトよりリセット処理とハイスコア判定処理を呼び出す
        //StageManager stageManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<StageManager>();
        //stageManager.ResetStageCount(1);
        //StartCoroutine(stageManager.CheckHighScore(3.0f));
        // エクセレント回数をリセットする
        StageManager.excelentCount = 0;
        Debug.Log(StageManager.excelentCount);
        // リトライ確認ポップアップを生成し、開く
        StartCoroutine(CreateRetryPopup());
    }

    /// <summary>
    /// リトライ確認ポップアップを生成して開く処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateRetryPopup() {
        // ゲームオーバーリザルト表示後に生成する
        yield return new WaitForSeconds(2.5f);
        RetryPopupManager retryPopUpWindow = Instantiate(retryPopup, GameObject.FindGameObjectWithTag("Canvas").transform, false);
        retryPopUpWindow.OpenPopUp();
    }

    /// <summary>
    /// ステージクリア処理。ライフ減少を停止。
    /// ステージクリアの状態に応じてフラグを立てる
    /// </summary>
    public void Clear(){
        if(gameState != GameState.GAME_OVER) {
            //  クリア後、ライフ減少を止める
            loss = 0f;
            if(isHungry) {
                // クリアフラグを立ててリザルトテキスト表示
                gameState = GameState.STAGE_CLEAR;
                GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ResultDisplayManager>().DisplayClearText(false);
                // witch0以外はノーマルクリアだとエクセレントの回数が途切れる
                if(GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchMove>().charaNum != 0) {
                    StageManager.excelentCount = 0;
                    Debug.Log(StageManager.excelentCount);
                }
            } else {
                // エクセレントフラグを立ててリザルトテキスト表示
                gameState = GameState.EXCELENT;
                GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ResultDisplayManager>().DisplayClearText(true);
                // ボーナスステージ以外ならエクセレント回数を加算
                if(SceneManager.GetActiveScene().name != "Bonus_Stage") {
                    StageManager.excelentCount++;
                    Debug.Log(StageManager.excelentCount);
                }
                if(StageManager.excelentCount >= 5 && !CharaSet.achievements[3]){
                    // witch4追加
                    CharaSet.achievements[3] = true;
                    CharaSet.SaveAchievement(3);
                }
            }
        }
    }
}
