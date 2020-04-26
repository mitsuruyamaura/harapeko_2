using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBurstPopup : MonoBehaviour {

    [Header("ブーストポップアップ制御用")]
    public GameObject burstPopUp;
    [Header("広告管理")]
    public AdMobInterstitial inste;
    [Header("デバッグ切り替え。trueならデバッグオン")]
    public bool isDebug;
    private float admobTimer;
    public Text infoTxt;
    private bool isSelectionButton;   // 同じボタン・異なるボタンを重複して押せないようにするフラグ

    public static bool isBurst;       // ブースト用フラグ（仮）

    /// <summary>
    /// スコアブーストポップアップを開く
    /// </summary>
    /// <returns></returns>
    public void OpenPopUp() {
        Vector3 scalePos = burstPopUp.transform.localScale;    //  拡大・縮小表示する描画用オブジェクトのScale値を取得する
        burstPopUp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);          //  拡大・縮小表示する描画用オブジェクトのScale値を最小値にする
        iTween.ScaleTo(burstPopUp, iTween.Hash("x", scalePos.x, "y", scalePos.y, "z", scalePos.z, "time", 1.0f));  //  元の大きさまでアニメで表示
    }

    /// <summary>
    /// インステ広告を再生する処理
    /// </summary>
	public void OnClickAdPlayBtn() {
        if (!isSelectionButton) {
            if (!isDebug) {
                inste.interstitialAd.Show();
            } else {
                // 代わりに30秒間のカウントダウンを表示する
                admobTimer = 2.0f;
            }
            isSelectionButton = true;
            // TODO ブースト用のフラグを立てる
            isBurst = true;
        }
    }

    /// <summary>
    /// ウインドウを閉じる処理
    /// </summary>
    public void OnClickReturnBtn() {
        if (!isSelectionButton) {
            Vector3 scalePos = burstPopUp.transform.localScale;                          //  拡大・縮小表示する描画用オブジェクトのScale値を取得する
            burstPopUp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);             //  拡大・縮小表示する描画用オブジェクトのScale値を最小値にする
            iTween.ScaleFrom(burstPopUp, iTween.Hash("x", scalePos.x, "y", scalePos.y, "z", scalePos.z, "time", 0.5f));  //  元の大きさまでアニメで表示
            Destroy(gameObject, 0.5f);
        }
    }

    private void Update() {
        if (admobTimer > 0) {
            admobTimer -= Time.deltaTime;
            infoTxt.text = "広告表示中 ...\nあと " + admobTimer.ToString("F0") + " 秒";
            if (admobTimer <= 0) {
                infoTxt.text = "ブースト 完了!!";
                Destroy(gameObject,1.0f);
            }
        }
    }
}
