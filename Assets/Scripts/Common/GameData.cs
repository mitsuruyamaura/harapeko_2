using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [Header("インステブースト用")]
    public bool isAdMobBoost;
    [Header("広告デバッグ切り替え。trueならデバッグオン")]
    public bool isAdmobDebug;
    [Header("使用言語(0=JP,1=EG,2=CH)")]
    public int language;

    [Header("クリアした回数")]
    public int gameClearCount;
    [Header("クリアしたステージ数の管理数値")]
    public int stage = 1;
    [Header("エクセレントでクリアした回数")]
    public int excelentCount;
    [Header("難易度(0=Normal,1=Hard)")]
    public int difficulty;
    [Header("使用キャラ番号")]
    public int selectCharaNum;


    public float score;              //  シーン間で引き継ぐため
    public float highScore;　　　　　//　シーン間で引き継ぐため

    public bool isNewRecord;         //  ハイスコアが更新されたか判定。trueならハイスコアポップアップ表示処理オン

    public float initScore;　　　　　// ステージ開始時のscore保存用
    public float initHighScore;      // ステージ開始時のhighscore保存用
    public bool isRetry;             // リトライ確定フラグ。RetryPopupのインステ広告再生後にtrueになる

    [Header("開放済キャラクター管理")]
    public bool[] achievements = new bool[5];

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        Setup();
    }

    private void Setup() {
        //  ゲームのクリア回数を取得し、スコアや難易度などをリセット
        gameClearCount = PlayerPrefs.GetInt("clearCount", 0);
        Debug.Log(gameClearCount);


    }

}
