using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobBanner : MonoBehaviour {

    public static AdMobBanner instance;

    [SerializeField,Header("アプリID(デフォルトはテスト値)")]
    public string appId = "ca-app-pub-3940256099942544~3347511713";
    [SerializeField, Header("バナーの広告ユニットID(デフォルトはテスト値)")]
    public string adUnitId = "ca-app-pub-3940256099942544/6300978111";

    public bool isAdmob;                       // 広告を見たかどうかのフラグ
    public BannerView bannerView;
    public static int AdMobCount;              // 広告を見た通算カウント。保存する
    public static int currentAdMobCount;       // 起動中に広告を見た通算カウント。保存しない

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }     
    }

    void Start () {
        // TODO AndroidManifest.xml(NCMB)もテストアプリIDになっているので使用時には変更する
        //string appId = "ca-app-pub-3444925805401185~2047163081";
        // 初期化(Google Mobile Ads SDK)
        MobileAds.Initialize(appId);
        RequestBanner();
	}
	
	private void RequestBanner() {
        //string adUnitId = "ca-app-pub-3444925805401185/9592688609";
        // ボトムにバナー広告を設定
        bannerView = new BannerView(adUnitId,AdSize.Banner, AdPosition.Top);
        // 空のAdRequestを作る
        AdRequest request = new AdRequest.Builder().Build();
        // リクエストを読み込む
        bannerView.LoadAd(request);
    }
}
