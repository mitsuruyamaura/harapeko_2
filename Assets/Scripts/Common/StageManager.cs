using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour {

    [SerializeField, Header("ハイスコアポップアップ表示")]
    public GameObject highScorePopup;

    public static int gameClearCount;     //  クリアした回数
    public static int stage = 1;          //  クリアしたステージ数の管理数値
    public static int excelentCount;      //  エクセレントでクリアした回数
    public static int difficulty;         //  難易度管理
    public static int selectCharaNum;     //  選択しているキャラ
    public static int language;           //  使用言語

    private string CLEAR_COUNT = "clearCount";

    void Awake() {
        //  ゲームのクリア回数を取得し、スコアや難易度などをリセット
        if(SceneManager.GetActiveScene().name == "Title") {
            gameClearCount = PlayerPrefs.GetInt("clearCount", 0);
            Debug.Log(gameClearCount);
            ResetStageCount(1);
        }
    }

    /// <summary>
    /// ゲームの難易度によりスタートステージの分岐処理
    /// </summary>
    /// <param name="difficulty"></param>
    public IEnumerator StartStage(int difficultyNum) {
        // トランジション用のフェイドアウト処理を入れる
        GameObject.FindGameObjectWithTag("FadeCanvas").GetComponent<FadeManager>().TransitionStage(0.7f);
        yield return new WaitForSeconds(0.7f);
        difficulty = difficultyNum;
        if(difficulty == 0) {
            // のーまる
            SceneManager.LoadScene("Stage_1");
        } else {
            // はーど
            SceneManager.LoadScene("Stage_6");
        }
    }

    /// <summary>
    /// 次ステージを呼び出す処理。ステージクリア時に呼ばれる
    /// </summary>
    /// <param name="stageCount"></param>
    public void NextStage(int stageCount) {
        //　コルーチンを呼び出す
        StartCoroutine(Next(stageCount));
    }

    /// <summary>
    /// ステージ遷移処理
    /// </summary>
    /// <param name="stageCount"></param>
    /// <returns></returns>
    IEnumerator Next(int stageCount) {
        if(!DebugSwitch.isTitleReturnFlg && GameObject.FindGameObjectWithTag("HP").GetComponent<Life>().gameState != Life.GameState.GAME_OVER) {
            // デバッグフラグがなく、ゲームオーバーでなければ次のステージへ移行
            stage += stageCount;
            Debug.Log(stage);
        } else {
            // デバッグフラグオンならクリア時に常にタイトルに戻す
            stage = stageCount;
            Debug.Log(stage);
        }
        yield return new WaitForSeconds(10f);
        // ボーナスステージ以外ならトランジション用のフェイドアウト処理を入れる
        if (stage != 7) {
            GameObject.FindGameObjectWithTag("FadeCanvas").GetComponent<TransitionManager>().TransFadeOut(0.7f);
        }
        yield return new WaitForSeconds(0.7f);
        // ローカルのhighscorのセーブ
        GetComponent<Score>().Save();       
        if (difficulty == 0) {
            switch(stage) {
                case 1:
                    // デバッグ用。リセットしてタイトルへ戻す。ハイスコア更新確認なし
                    ResetStageCount(1);
                    ReturnTitle();
                    break;
                case 2:
                    SceneManager.LoadScene("Stage_2");
                    break;
                case 3:
                    SceneManager.LoadScene("Stage_3");
                    break;
                case 4:
                    SceneManager.LoadScene("Stage_4");
                    break;
                case 5:
                    SceneManager.LoadScene("Stage_5");
                    break;
                case 6:
                    SceneManager.LoadScene("Bonus_Stage");
                    break;
                case 7:
                    if(gameClearCount >= 0 && !CharaSet.achievements[1]){
                            // witch2追加
                            CharaSet.achievements[1] = true;
                            CharaSet.SaveAchievement(1);
                    }
                    gameClearCount++;
                    SaveClearCount();
                    ResetStageCount(1);
                    StartCoroutine(CheckHighScore());
                    break;                
            }
        }
        if(difficulty == 1) {
            switch(stage) {
                case 2:
                    SceneManager.LoadScene("Stage_7");
                    break;
                case 3:
                    SceneManager.LoadScene("Stage_8");
                    break;
                case 4:
                    SceneManager.LoadScene("Stage_9");
                    break;
                case 5:
                    SceneManager.LoadScene("Stage_10");
                    break;
                case 6:
                    SceneManager.LoadScene("Bonus_Stage");
                    break;
                case 7:
                    if(gameClearCount >= 1 && !CharaSet.achievements[2]) {
                            // witch3追加
                            CharaSet.achievements[2] = true;
                            CharaSet.SaveAchievement(2);
                    }
                    gameClearCount++;
                    SaveClearCount();
                    ResetStageCount(1);
                    StartCoroutine(CheckHighScore());
                    break;
            }
        }
    }

    /// <summary>
    /// ステージ周りのリセット処理
    /// ゲーム起動時、全ステージクリア時、ゲームオーバー時にタイトルへ戻るから呼び出し
    /// </summary>
    /// <param name="stagereset = 1"></param>
    public void ResetStageCount(int stagereset) {
        stage = stagereset;
        excelentCount = 0;
        difficulty = 0;
    }

    /// <summary>
    /// ハイスコアの判定処理
    /// ゲームオーバー時タイトルへ戻るから呼び出し、ステージクリア時に呼び出し
    /// ハイスコア更新ならポップアップ表示
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckHighScore(float waitCount = 0.0f) {
        // ゲームオーバー時のみwaitCountが入り3秒待機
        yield return new WaitForSeconds(waitCount);
        if(!DebugSwitch.isHighscoreFlg && Score.isNewRecord) {
            // 更新のスキップオフでハイスコア更新ならリザルト表示を消してポップアップ表示
            GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ResultDisplayManager>().HiddenResultText();
            SetHighScore();
            yield break;
        } else {
            // ハイスコアの更新がない場合、待機後にタイトルに自動で戻す
            yield return new WaitForSeconds(waitCount);  // 10.0f + waitCount
            StartCoroutine(ReturnTitle());
        }
    }

    /// <summary>
    /// サーバーへのハイスコア保存処理
    /// 全ステージクリア時、ゲームオーバー時に呼び出し
    /// ハイスコアを更新していればポップアップ表示しサーバーに保存
    /// </summary>
    /// <returns></returns>
    public void SetHighScore() {
        GameObject highScorePopUpWindow = Instantiate(highScorePopup, GameObject.FindGameObjectWithTag("PopUpCanvas").transform, false);
        highScorePopUpWindow.GetComponent<HighScorePopupManager>().OpenPopUp();
    }

    /// <summary>
    /// タイトル画面へ遷移する処理
    /// デバッグスイッチオン時、ハイスコアの更新がなかった場合、
    /// ハイスコアポップアップを閉じた際に呼び出し
    /// </summary>
    public IEnumerator ReturnTitle() {
        // トランジション用フェイドイン処理
        GameObject.FindGameObjectWithTag("FadeCanvas").GetComponent<TransitionManager>().TransFadeOut(0.7f);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene("Title");
    }

    /// <summary>
    /// クリア回数の保存処理
    /// </summary>
    private void SaveClearCount() {
        PlayerPrefs.SetInt(CLEAR_COUNT, gameClearCount);
        Debug.Log(gameClearCount);
        PlayerPrefs.Save();
        Debug.Log("Save to gameClearCount");
    }
    /// <summary>
    /// サーバーのプレイヤー名からハイスコアを取得する
    /// 未使用
    /// </summary>
    //public void CheckRemoteHighScore() {
    //    // リモートのハイスコアを取得する。保存されていなければ0を取得する
    //    string name = FindObjectOfType<UserAuth>().CurrentPlayer();
    //    if(name != null) {
    //        // ログインしているなら
    //        NCMB.HighScore rankingHighScore = new NCMB.HighScore(0, name);
    //        rankingHighScore.Fetch();
    //        // リモートのハイスコアとローカルのハイスコアを比較して更新する
    //        if(rankingHighScore.score < highScore) {
    //            rankingHighScore.score = highScore;
    //            rankingHighScore.Save();
    //        }
    //    }
    //}
}
