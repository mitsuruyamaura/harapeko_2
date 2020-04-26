using UnityEngine;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour {

    [Header("ゲーム終了確認のダイアログ")]
    public GameObject exitDialog;
    [Header("確認用テキスト")]
    public Text noticeTxt;
    [Header("イエスボタンテキスト")]
    public Text yesBtnTxt;
    [Header("ノーボタンテキスト")]
    public Text noBtnTxt;

    public static GameEndManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }


    void Start(){
        exitDialog.SetActive(false);             //  ダイアログを非表示にする
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape)){    //  ESCを押したとき
            if (exitDialog.activeSelf == false){  //  終了のダイアログが開いていないなら
                ExitChoice();                     //  ゲームを終了するダイアログをアクティブにする
            } else {
                OnCancel();                       //  開いていたなら非アクティブにする
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftAlt)) {   //  ハイスコアとフラグの外部リセット用
            Reset();
        }
    }

    /// <summary>
    /// スコアやフラグ等の全リセット処理
    /// </summary>
    public void Reset() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Reset");
    }

    /// <summary>
    /// 終了確認のダイアログ表示
    /// EscかExitボタンから呼ばれる
    /// </summary>
    public void ExitChoice(){
        exitDialog.SetActive(true);
        switch(StageManager.language){
            case 0 :
                noticeTxt.text = "遊んで くれて ありがとう !\n\nアプリ を 終了 しますか ？";
                yesBtnTxt.text = "終了 する";
                noBtnTxt.text = "まだ 遊ぶ ！";
                break;
            case 1:
                noticeTxt.text = "Thank you for playing !\n\nAlright, do you want to exit the app ?";
                yesBtnTxt.text = "I will finish.";
                noBtnTxt.text = "I will still play !";
                break;
            case 2:
                noticeTxt.text = "谢谢你的参与 !\n\n你想退出应用程序吗 ？";
                yesBtnTxt.text = "退出";
                noBtnTxt.text = "还在玩 !";
                break;
        }
        // ゲーム時間の流れを停止
        Time.timeScale = 0f;
    }

    /// <summary>
    /// キャンセル処理。EscかNoボタンで呼ばれる
    /// </summary>
    public void OnCancel(){
        exitDialog.SetActive(false);
        // ゲーム時間の流れを再開
        Time.timeScale = 1.0f;
    }

    /// <summary>
    /// ゲーム終了処理
    /// Yesボタンより呼ばれる
    /// </summary>
    public void GameEnd(){
        //  ゲームの実行状況に合わせて処理を行う

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_WEBGL
        Application.OpenURL("https://www.yahoo.co.jp/");

#else //UNITY_STANDALONE
        Application.Quit();

#endif
    }
}
