using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("チュートリアルポップアップ")]
    public GameObject tutorialPopup;
    [Header("戻るボタン")]
    public GameObject returnBtn;
    [Header("画面遷移用フェイドアウト／イン制御")]
    public FadeManager fadeManager;

    /// <summary>
    /// チュートリアルウィンドウへ画面遷移処理
    /// 遊び方ボタンより呼び出し
    /// </summary>
    public void OpenTutorialPopup() {
        fadeManager.TransitionTutorial();
        StartCoroutine(DisplayTutorial());
    }

    /// <summary>
    /// チュートリアルウィンドウを表示する
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayTutorial() {
        yield return new WaitForSeconds(1.0f);
        tutorialPopup.SetActive(true);
        returnBtn.SetActive(true);
    }

    /// <summary>
    /// チュートリアルウィンドウを非表示にする
    /// タイトル画面への遷移処理
    /// 戻るボタンから呼び出し
    /// </summary>
    public void CloseTutorialPopUp() {
        tutorialPopup.SetActive(false);
        returnBtn.SetActive(false);
        fadeManager.TransitionTitle(0.7f);
    }
}
