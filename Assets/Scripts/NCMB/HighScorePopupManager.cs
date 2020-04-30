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
        transform.DOScale(1.0f, 0.5f);
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
            HighScore high = new HighScore(GameData.instance.highScore, playerName);
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
        transform.DOScale(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        // タイトル画面へ戻る
        isSend = false;
        StageManager stageManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<StageManager>();
        StartCoroutine(stageManager.ReturnTitle());
    }
}
