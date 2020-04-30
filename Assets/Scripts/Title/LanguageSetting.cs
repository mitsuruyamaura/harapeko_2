using UnityEngine;
using UnityEngine.UI;

public class LanguageSetting : MonoBehaviour {

    [Header("選択言語ボタンテキスト")]
    public Text selectLanguage;
    [Header("スタートボタンテキスト")]
    public Text startBtnTxt;
    [Header("ランキングボタンテキスト")]
    public Text rankingBtnTxt;
    [Header("遊び方ボタンテキスト")]
    public Text tutorialBtnTxt;
    [Header("広告ボタンテキスト")]
    public Text advertiseBtnTxt;
    [Header("メインタイトルテキスト")]
    public Text mainTitleTxt;
    [Header("サブタイトルテキスト")]
    public Text subTitleTxt;

    private const string LANGUAGE_NUM = "language";

    private void Start() {
        // 保存されている言語を取得。デフォルトは日本語
        GameData.instance.language = PlayerPrefs.GetInt(LANGUAGE_NUM, 0);
        DisplaySelectLanguage();
    }

    /// <summary>
    /// 使用言語の切り替え処理
    /// 設定ボタンを押す度に切り替える
    /// </summary>
    public void OnClickChangeLanguageSetting() {
        GameData.instance.language++;
        if (GameData.instance.language >= 3) {
            GameData.instance.language = 0;          
        }
        PlayerPrefs.SetInt(LANGUAGE_NUM, GameData.instance.language);
        PlayerPrefs.Save();
        DisplaySelectLanguage();
    }

    /// <summary>
    /// 選択している言語でタイトル画面のテキスト周りを表示
    /// </summary>
    public void DisplaySelectLanguage() {
        switch (GameData.instance.language) {
            case 0:
                selectLanguage.text = "日本語";
                startBtnTxt.text = "スタート!!";
                rankingBtnTxt.text = "ランキング";
                tutorialBtnTxt.text = "遊び方";
                advertiseBtnTxt.text = "スコアブースト!!";
                mainTitleTxt.text = "はらぺこウィッチ！";
                subTitleTxt.text = "プラス";
                break;
            case 1:
                selectLanguage.text = "ENGLISH";
                startBtnTxt.text = "START!!";
                rankingBtnTxt.text = "RANKING";
                tutorialBtnTxt.text = "HOW TO PLAY";
                advertiseBtnTxt.text = "ADVERTISING\nBENEFITS!!";
                mainTitleTxt.text = "Hungry Witches！";
                subTitleTxt.text = "Plus";
                break;
            case 2:  // 中国　簡体字 TODO （繁体字は？）
                selectLanguage.text = "简体中文";
                startBtnTxt.text = "启动!!";
                rankingBtnTxt.text = "排行";
                tutorialBtnTxt.text = "教程";
                advertiseBtnTxt.text = "广告效果!!";
                mainTitleTxt.text = "饥饿的女巫！";
                subTitleTxt.text = "Plus";
                break;
        }
    }
}
