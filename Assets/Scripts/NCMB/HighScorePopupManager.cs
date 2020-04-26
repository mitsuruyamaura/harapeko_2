using NCMB;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HighScorePopupManager : MonoBehaviour {

    [Header("メインポップアップ")]
    public GameObject popUp;
    [Header("ハイスコア表示用")]
    public ViewHighScore viewHighScore;
    [Header("前入力取得用")]
    public GetInputText getInput;
    [Header("トースト表示ポップアップ")]
    public GameObject toastPopUp;

    private string playerName;
    private bool isSend = false;   // ボタン連弾による重複送信防止

    /// <summary>
    /// アニメ付きでハイスコアポップアップ表示
    /// </summary>
    public void OpenPopUp() {
        Vector3 scalePos = popUp.transform.localScale;    //  拡大・縮小表示する描画用オブジェクトのScale値を取得する
        popUp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);          //  拡大・縮小表示する描画用オブジェクトのScale値を最小値にする
        iTween.ScaleTo(popUp, iTween.Hash("x", scalePos.x, "y", scalePos.y, "z", scalePos.z, "time", 2.0f));  //  元の大きさまでアニメで表示
        viewHighScore.DipslayHighScore();
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnClickSaveBtn() {
        StartCoroutine(GetInputPlayerName());
    }

    /// <summary>
    /// InputFieldで入力されたプレイヤー名を取得してサーバーに送信してセーブする
    /// 送信ボタンで呼ばれる
    /// </summary>
    private IEnumerator GetInputPlayerName() {     
        if(!isSend) {
            isSend = true;
            playerName = getInput.GetText();
            HighScore high = new HighScore(Score.highScore, playerName);
            IEnumerator coroutine = high.Save(high);
            yield return StartCoroutine(coroutine);
            StartCoroutine(ClosePopUp((bool)coroutine.Current));
        }
    }

    /// <summary>
    /// アニメ付きでポップアップ表示を閉じる
    /// 受け取ったbool値によってセーブ可否を表示する
    /// タイトル画面に遷移する
    /// </summary>
    /// <returns></returns>
    IEnumerator ClosePopUp(bool isSaved) {
        // セーブに成功したか、失敗したかをトーストメッセージにして表示
        toastPopUp.SetActive(true);
        toastPopUp.GetComponent<ToastText>().DisplayToast(isSaved);
        yield return new WaitForSeconds(1.0f);
        Vector3 scalePos = popUp.transform.localScale;                          //  拡大・縮小表示する描画用オブジェクトのScale値を取得する
        popUp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);             //  拡大・縮小表示する描画用オブジェクトのScale値を最小値にする
        iTween.ScaleFrom(popUp, iTween.Hash("x", scalePos.x, "y", scalePos.y, "z", scalePos.z, "time", 0.8f));  //  元の大きさまでアニメで表示
        yield return new WaitForSeconds(0.8f);
        // タイトル画面へ戻る
        isSend = false;
        StageManager stageManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<StageManager>();
        StartCoroutine(stageManager.ReturnTitle());
    }
}
