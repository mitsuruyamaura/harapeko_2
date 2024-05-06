using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreBoostPopup : MonoBehaviour {

    // UI
    public Text infoTxt;
    public Button btnClose;
    public Button btnAdmobInste;

    private float admobTimer;
    private bool isClickable;   // 同じボタン・異なるボタンを重複して押せないようにするフラグ

    void Start() {
        // ボタン設定
        btnClose.onClick.AddListener(OnClickClose);
        btnAdmobInste.onClick.AddListener(OnClickAdmobPlay);      
    }

    /// <summary>
    /// スコアブーストポップアップを開く
    /// </summary>
    /// <returns></returns>
    public void OpenPopUp() {
        transform.DOScale(1.0f, 0.5f);
    }

    /// <summary>
    /// インステ広告を再生する処理
    /// </summary>
	public void OnClickAdmobPlay() {
        if (isClickable) {
            return;
        }
        isClickable = true;
        if (!GameData.instance.isAdmobDebug) {
            // インステ再生
            //AdMobInterstitial.instance.OnClickStartInterstitial();
        } else {
            // 代わりに30秒間のカウントダウンを表示する
            admobTimer = 2.0f;
        }
        // TODO ブースト用のフラグを立てる
        GameData.instance.isAdMobBoost = true;
    }

    /// <summary>
    /// ウインドウを閉じる
    /// </summary>
    public void OnClickClose() {
        if (isClickable) {
            return;
        }
        isClickable = true;
        transform.DOScale(0, 0.5f);
        Destroy(gameObject, 0.5f);
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
