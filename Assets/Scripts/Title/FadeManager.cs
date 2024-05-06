using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面内のトランジション用クラス
/// ゲーム画面へのトランジションも含む
/// </summary>
public class FadeManager : MonoBehaviour {

    [Header("キャンバス制御用")]
    public Canvas canvas;
    [Header("フェイドイン／フェイドアウト制御用")]
    public Fade fade;
    [Header("初期キャラ表示制御用")]
    public CharaSet charaSet;
    [Header("タイトル画面制御用")]
    public Title title;
    
    private bool isCharaSet;

    [Header("タイトル画面マスク用パネル")]
    public GameObject maskPanel;

    /// <summary>
    /// フェイドイン／フェイドアウトの処理
    /// FadeIn()の引数ににActionのデリゲードがある
    /// </summary>
    void Start() {
        TransitionTitle(1.0f);
        // マスク用パネルを破壊する
        Destroy(maskPanel, 1.0f);
    }

    /// <summary>
    /// タイトル画面へのフェードイン／フェードアウト遷移処理
    /// 戻るボタンから呼び出し
    /// </summary>
    public void TransitionTitle(float time) {
        StartCoroutine(HiddenTitleImage(0.7f));
        canvas.enabled = false;
        fade.FadeIn(time, () =>
        {         
            fade.FadeOut(time);
            if (!isCharaSet) {
                charaSet.SetUp();
                isCharaSet = true;
            }
            canvas.enabled = true;
            //AdMobBanner.instance.bannerView.Show();
            title.DisplayTitleMode();
        });
    }

    /// <summary>
    /// タイトル画面を一度非表示にする処理
    /// </summary>
    private IEnumerator HiddenTitleImage(float wait) {
        yield return new WaitForSeconds(wait);
        title.titleImage[0].enabled = false;
        title.titleImage[1].enabled = false;
    }

    /// <summary>
    /// チュートリアル／ランキング画面へのフェードイン／フェードアウト遷移処理
    /// 各ボタンから呼び出し
    /// </summary>
    public void TransitionTutorial() {
        HiddenTitleImage(0.5f);
        canvas.enabled = false;
        fade.FadeIn(0.7f, () =>
        {
            // バナー広告を非表示にする
            //AdMobBanner.instance.bannerView.Hide();
            fade.FadeOut(0.7f);
            title.DisplayChangeImage();
        });
    }

    /// <summary>
    /// タイトル画面からステージに遷移する際のトランジション処理
    /// </summary>
    /// <param name="time"></param>
    public void TransitionStage(float time) {
        // バナー広告を破壊する
        //AdMobBanner.instance.bannerView.Destroy();
        fade.FadeIn(0.7f, () =>
        {
            fade.FadeOut(time);
        });
    }
}
