using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharaSet : MonoBehaviour {

    [Header("キャラクター選択用ポップアップ")]
    public GameObject charaSelectPopUp;
    
    [Header("詳細表示テキスト")]
    public Text detailText;
    [Header("ハードモードボタン制御用")]
    public Button hardModeBtn;
    [Header("ハードモードテキスト制御用")]
    public Text hardModeText;
    [Header("ノーマルモードテキスト制御用")]
    public Text normalModeText;

    [Header("キャラクター用 おかしアイコン")]
    public GameObject[] icons;
    [Header("未開放キャラ用 ？アイコン マスク")]
    public GameObject [] masks;
    [Header("選択キャラ表示")]
    public GameObject[] charas;
    [Header("キャラの足場")]
    public GameObject[] grounds;

    [Header("デバッグモード用")]
    public DebugSwitch debug;

    private Vector3 originalSize;
    private const string WITCH_ACHIEVEMENT = "witch_";

    public CanvasGroup buttonsCanvasGroup;

    /// <summary>
    /// 追加キャラの確認と初期キャラのセット処理
    /// FadeManagerより呼び出し
    /// </summary>
    public void SetUp() {
        originalSize = charaSelectPopUp.transform.localScale;
        charaSelectPopUp.transform.localScale = Vector3.zero;

        LoadAchievement();
        // 初回起動時のみ初期キャラを使用可能キャラに設定し保存
        if (!GameData.instance.achievements[0]) {
            GameData.instance.achievements[0] = true;
            SaveAchievement(0);
            Debug.Log("witch_1");
        }
        charaSelectPopUp.SetActive(false);
        CheckNormalModeCleared();
        SelectChara(-1);
        if (debug.debugModeOn) {
            // デバッグスイッチがオンならデバッグモードを設定する
            debug.DebugSetup();
            Debug.Log("DebugMode : ON");
        }
    }

    /// <summary>
    /// クリア状態の判定処理
    /// </summary>
    private void CheckNormalModeCleared() {
        switch (GameData.instance.language) {
            case 0:
                normalModeText.text = "のーまる コースで\nゲームスタート!!";
                break;
            case 1:
                normalModeText.text = "Start a\nNormal course!!";
                break;
            case 2:
                normalModeText.text = "开始\n正常的课程!!";
                break;
        }        
        if (GameData.instance.gameClearCount <= 0) {
            hardModeBtn.interactable = false;
            switch (GameData.instance.language) {
                case 0:
                    hardModeText.text = "はーど コース\n未開放";
                    break;
                case 1:
                    hardModeText.text = "Hard course is\nnot open.";
                    break;
                case 2:
                    hardModeText.text = "艰难的过程\n是不公开的";
                    break;
            }        
        } else {
            hardModeBtn.interactable = true;
            switch (GameData.instance.language) {
                case 0:
                    hardModeText.text = "はーど コースで\nゲーム スタート!!";
                    break;
                case 1:
                    hardModeText.text = "Start a\nHard course!!";
                    break;
                case 2:
                    hardModeText.text = "从艰难的\n课程开始!!";
                    break;
            }
        }
    }

    /// <summary>
    /// キャラ選択ポップアップを表示
    /// </summary>
    public void OnClickStartButton() {
        // バナー広告や各種ボタンを非表示にする
        AdMobBanner.instance.bannerView.Hide();
        buttonsCanvasGroup.alpha = 0;
        charaSelectPopUp.SetActive(true);
        // 追加キャラの取得とコースクリア状態の再取得
        LoadAchievement();
        CheckNormalModeCleared();
        transform.DOScale(originalSize, 0.5f);

        CreateIcon();
        GameData.instance.selectCharaNum = -1;
    }

    /// <summary>
    /// 開放済キャラと未開放キャラを表示
    /// 対応するフラグで分岐して表示
    /// </summary>
    private void CreateIcon() {
        icons[0].SetActive(true);
        if(GameData.instance.achievements[1]) {
            icons[1].SetActive(true);
        } else {
            masks[0].SetActive(true);
        }
        if(GameData.instance.achievements[2]) {
            icons[2].SetActive(true);
        } else { 
            masks[1].SetActive(true);
        }
        if(GameData.instance.achievements[3]) {
            icons[3].SetActive(true);
        } else {
            masks[2].SetActive(true);
        }
        if(GameData.instance.achievements[4]) {
            icons[4].SetActive(true);
        } else {
            masks[3].SetActive(true);
        }
    }

    /// <summary>
    /// プレイヤーとするキャラの決定
    /// キャラを押す度に選択しているキャラを確定
    /// 画面のアイコンを押すと呼び出し。intはボタンから取得
    /// </summary>
    public void SelectChara(int num) {
        GameData.instance.selectCharaNum = num;
        ViewDetailText(GameData.instance.selectCharaNum);
    }

    /// <summary>
    /// 未開放のキャラ選択時のテキスト表示
    /// マスクのアイコンを押すと表示される
    /// </summary>
    public void ViewAchievedDetail(int num) {
        GameData.instance.selectCharaNum = -1;
        for(int i = 0; i < charas.Length; i++) {
            // キャラと足場を消す
            charas[i].SetActive(false);
            grounds[i].SetActive(false);
        }
        switch(num) {
            case 0:
                detailText.text = "のーまる コース クリアで キャラが 開放されます。";
                break;
            case 1:
                detailText.text = "はーど コース クリアで キャラが 開放されます。";
                break;
            case 2:
                detailText.text = "のーまる コースか はーど コースのどちらかで\nすべてエクセレントを取ってクリアすると キャラが 開放されます。";
                break;
            case 3:
                detailText.text = "ハイスコア 200,000点以上で キャラが 開放されます。";
                break;
        }
    }

    /// <summary>
    /// キャラ選択時の詳細表示
    /// </summary>
    /// <param name="num"></param>
    private void ViewDetailText(int num) {
        for(int i = 0; i < charas.Length; i++) {
            // キャラと足場を消して選択キャラを表示
            if(i == num) {
                charas[i].SetActive(true);
                grounds[i].SetActive(false);
            } else {
                charas[i].SetActive(false);
                grounds[i].SetActive(false);
            }
        }
        // ランダムな足場を表示
        var randomValue = Random.Range(0,grounds.Length);
        grounds[randomValue].SetActive(true);
        switch(num) {
            case -1:
                detailText.text = "おかし(キャラ)を おしてから コースを おしてね。";
                break;
            case 0:
                detailText.text = "始めから使えるキャラです。\nこのキャラのみ、ノーマルクリアをしても\nエクセレント回数がリセットされません。";
                break;
            case 1:
                detailText.text = "移動速度が速くなりますが\nチャージ攻撃の ライフ消費が多くなります\n敵を倒したり アイテムを取った際のスコアが 10% 増えます。";
                break;
            case 2:
                detailText.text = "移動速度が遅くなりますが 攻撃力が 大きくなります\n敵を倒したり アイテムを取った際のスコアが 20% 増えます。";
                break;
            case 3:
                detailText.text = "お腹の空くのが早くなりますが\nチャージ攻撃のライフ消費が 少なくなります\n敵を倒したり アイテムを取った際のスコアが 40% 増えます。";
                break;
            case 4:
                detailText.text = "お腹が空くのがめちゃくちゃ早くなりますが\nチャージ攻撃のライフ消費が とても少なくなります\nまた 攻撃力が大きくなります。敵を倒したり アイテムを取った際のスコアが 100%(2倍) 増えます。";
                break;
        }
    }

    /// <summary>
    /// モード選択処理。のーまる、はーど、どちらも通る
    /// </summary>
    public void ChoiceDifficulty(int difficulty) {
        if(CheckSelectedChara()) {
            StageManager stageManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<StageManager>();
            StartCoroutine(stageManager.StartStage(difficulty));
        }
    }

    /// <summary>
    /// スタート前の判定
    /// キャラ未選択の場合にははじく
    /// </summary>
    private bool CheckSelectedChara() {
        bool isCheck = false;
        if(GameData.instance.selectCharaNum < 0) {
            detailText.text = "おかし(キャラ)を 選択してから おしてね。";
        } else {
            isCheck = true;
        }
        return isCheck;
    }

    /// <summary>
    /// 追加キャラの保存
    /// bool配列をintにキャストして保存
    /// </summary>
    public static void SaveAchievement(int charaNum) {
        // achievements[]をint型にキャストして代入。trueなら1
        PlayerPrefs.SetInt(WITCH_ACHIEVEMENT + charaNum.ToString(), (GameData.instance.achievements[charaNum]) ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log(GameData.instance.achievements[charaNum]);
    }

    /// <summary>
    /// 追加キャラの読み込み
    /// </summary>
    private void LoadAchievement() {
        for(int i = 0; i < GameData.instance.achievements.Length; i++) {
            // witch_iが1かどうかを見てbool型にキャストして代入。1ならtrue
            GameData.instance.achievements[i] = (PlayerPrefs.GetInt(WITCH_ACHIEVEMENT + i.ToString(), 0) == 1);
            Debug.Log(GameData.instance.achievements[i]);
        }
        if (debug.debugModeOn) {
            // デバッグスイッチがオンなら追加キャラのデバッグモードを設定する
            debug.CharaDebugSetup();
            Debug.Log("CharaDebugMode : ON");
        }
    }

    /// <summary>
    /// キャラ選択ポップアップを閉じる処理
    /// </summary>
    public void CloseCharaSelectWindow() {
        StartCoroutine(CloseCharaPopUp());       
    }

    /// <summary>
    /// ポップアップを閉じる実処理
    /// ランキングボタンとスタートボタンとバナー広告を再表示する
    /// </summary>
    IEnumerator CloseCharaPopUp() {
        transform.DOScale(0, 0.5f);        
        yield return new WaitForSeconds(0.5f);

        charaSelectPopUp.SetActive(false);
        buttonsCanvasGroup.alpha = 1.0f;
        AdMobBanner.instance.bannerView.Show();
    }
}
