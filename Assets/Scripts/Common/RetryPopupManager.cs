using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class RetryPopupManager : MonoBehaviour {

    [Header("リトライポップアップ制御用")]
    public GameObject popUp;

    [Header("デバッグ切り替え。trueならデバッグオン")]
    public bool isDebug;
    private float admobTimer;
    public Text countTxt;
    private bool isSelectionButton;   // 同じボタン・異なるボタンを重複して押せないようにするフラグ

    /// <summary>
    /// リトライ確認ポップアップを開く
    /// </summary>
    /// <returns></returns>
    public void OpenPopUp () {
        transform.DOScale(1.0f, 0.5f);
    }

    /// <summary>
    /// リトライボタンを押した際の処理
    /// </summary>
    public void OnClickRetryBtn() {
        if (!isSelectionButton) {
            if (!isDebug) {
                transform.DOScale(0f, 0.5f);
            }
            // 広告を再生する
            StartCoroutine(InterstitialShow());
            isSelectionButton = true;
        }       
    }

    /// <summary>
    /// タイトルボタンを押した際の処理
    /// </summary>
    public void OnClickTitleBtn() {
        if (!isSelectionButton) {
            // ローカルにハイスコアを保存
            GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<Score>().Save();
            // StageManagerスクリプトよりリセット処理とハイスコア判定処理を呼び出す
            StageManager stageManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<StageManager>();
            stageManager.ResetStageCount(1);
            StartCoroutine(stageManager.CheckHighScore(0.5f));
            isSelectionButton = true;
        }
    }

    void Update() {
        if (admobTimer > 0) {
            admobTimer -= Time.deltaTime;
            countTxt.text = "広告表示中 ...\nあと " + admobTimer.ToString("F0") + " 秒";
            if (admobTimer <= 0) {
                countTxt.text = " ";
                admobTimer = 0;
                StartCoroutine(Retry());
            }
        }
        // 広告再生後にtrueになる
        //if (inste.isShow) {
        //    inste.isShow = false;
        //    StartCoroutine(Retry());
        //}
    }

    /// <summary>
    /// リトライ処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Retry() {
        GameObject.FindGameObjectWithTag("FadeCanvas").GetComponent<TransitionManager>().TransFadeOut(0.7f);
        yield return new WaitForSeconds(0.7f);
        // 同じゲームシーンをリロードする
        string reloadScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(reloadScene);
    }

    /// <summary>
    /// 広告の再生準備処理
    /// </summary>
    /// <returns></returns>
    private  IEnumerator InterstitialShow() {
        yield return new WaitForSeconds(0.8f);
        if (!isDebug) {
            // インステ広告を再生する
            //AdMobInterstitial.instance.OnClickStartInterstitial();
        } else {
            // 代わりに30秒間のカウントダウンを表示する
            admobTimer = 2.0f;
        }     
        GameData.instance.isRetry = true;
    }
}
