using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobInterstitial : MonoBehaviour {

    [SerializeField, Header("インステ用の広告ユニットID(デフォルトはテスト値)")]
    public string interstitial = "ca-app-pub-3940256099942544/1033173712";
    public InterstitialAd interstitialAd;
    public BannerView bannerView;
    public bool isShow;    // インステ広告の再生が終わったらtrueになる

	void Start () {
        // 起動時にインタースティシャル広告をロードしておく
        RequestInterstitial();
	}
	
    /// <summary>
    /// インタースティシャルをロードする
    /// </summary>
	public void RequestInterstitial() {
        // ユニットIDを入れて初期化
        interstitialAd = new InterstitialAd(interstitial);
        // 空のAdリクエストを作成
        AdRequest request = new AdRequest.Builder().Build();
        // インタースティシャルをロードしてリクエストに代入
        interstitialAd.LoadAd(request);
    }

    /// <summary>
    /// 再生の終わったインタースティシャル広告を破棄して再読み込み処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleAdClosed(object sender, System.EventArgs arges) {
        interstitialAd.Destroy();
        isShow = true;
        RequestInterstitial();
    }

    /// <summary>
    /// インタースティシャル広告を「閉じる」か「戻る」かした時に呼ばれる
    /// </summary>
    public void OnAdClosed() {
        // TODO 音楽などの再生
    }

    /// <summary>
    /// 広告ブーストボタンを押した時にインタースティシャルを再生する
    /// </summary>
    public void OnClickAdMobButton() {
        interstitialAd.Show();
    }
}
