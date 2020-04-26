using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Pause : MonoBehaviour {
    
    GameObject tutorial;
    Text tutorialText;
    Text tutorialText2;

    //  tutorialを見たかのフラグ状況の確認用
    int tutorialClear;                //  チュートリアルを見た
    int tutorialEnd = 0;　　　　　　　//  チュートリアルのテキストの何ページ目か

    void Start(){    
        //  チュートリアルのクリア状況を取得。保存していなければ0。クリアなら1。
        tutorialClear = PlayerPrefs.GetInt("TUTORIAL", 0);

        //  チュートリアルを見ていないなら
        if (tutorialClear == 0){
            //  各オブジェクトを取得
            tutorial = GameObject.Find("Tutorial");
            tutorialText = tutorial.transform.Find("TutorialText").gameObject.GetComponent<Text>();
            tutorialText2 = tutorial.transform.Find("TutorialText2").gameObject.GetComponent<Text>();
            //  ゲーム時間を止める
            Time.timeScale = 0f;
        } else {
            tutorial = GameObject.Find("Tutorial");
            ClearTutorial();
        }
    }

    void Update(){
        //  tutorialを見ていなかったら実行
        if (tutorialClear == 0){
            //  始めのテキスト表示
            if (tutorialEnd == 0){
                //  spaceか画面の★マークがタップされたら次のテキストを表示
                if (Input.GetKeyDown("z") || CrossPlatformInputManager.GetButtonDown("Action")){
                    //  前のテキストを非表示にして、次のテキストを表示。見たフラグを立てる
                    tutorialText.enabled = false;
                    tutorialText2.enabled = true;
                    tutorialEnd = 1;
                }     
            }

            //  ボタンを変えないと見なくても両方とも飛ばしてしまう
            //  ２ページ目のテキスト表示。フラグで状態確認
            if (tutorialEnd == 1){
                //  enterか画面の羽マークがタップされたらテキストを消去してゲームスタート
                if (Input.GetKeyDown("space") ||CrossPlatformInputManager.GetButtonDown("Jump")){
                    //  オブジェクト自体の表示を非アクティブにする
                    tutorial.SetActive(false);

                    //　次回移行は発生しない
                    //  チュートリアルの管理フラグ
                    PlayerPrefs.SetInt("TUTORIAL", 1);
                    PlayerPrefs.Save();
                    ClearTutorial();

                　   //　時間を流し始める
                    Time.timeScale = 1f;
                    return;
                }
            }
        }

        ////  チュートリアルを見ていればすぐにスタート
        //if(tutorialClear == 1){            
        //    //  オブジェクト自体の表示を非アクティブにする
        //    tutorial.SetActive(false);
        //}
    }

    /// <summary>
    /// チュートリアルを消してクリア目標を表示
    /// </summary>
    private void ClearTutorial() {
        tutorial.SetActive(false);
        //  チュートリアルが終了しているならリザルト情報を表示
        GameObject.FindGameObjectWithTag("HP").GetComponent<GameClearController>().DisplayResultText();
    }
}
