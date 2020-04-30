using UnityEngine;

public class DebugSwitch : MonoBehaviour {

    [SerializeField, Header("有効にするとデバッグモードオン 無効なら他のデバッグスイッチが入っていても無効")]
    public bool debugModeOn;

    [SerializeField, Header("trueで追加キャラが表示されるフラグ")]
    public bool[] charaFlg = new bool[4];
    [SerializeField, Header("ハイスコアの更新 スキップフラグ")]
    public bool isHighscore;
    [SerializeField, Header("trueでキーボードでキャラ操作可能にするフラグ")]
    public bool isKeyboardFlg;
    [SerializeField, Header("ステージクリア時にタイトルに戻すフラグ")]
    public bool isRetrunFlg;
    [SerializeField, Header("ステージ数とエクセレント回数とクリア回数と難易度のデバッグモードのON/OFF")]
    public bool isStageDebugSwitch;
    [SerializeField, Header("デバッグ用の現在のステージ数(1-5,ボーナスステージは 6)")]
    public int debugStageNum;
    [SerializeField, Header("デバッグ用のエクセレント回数(0-5)")]
    public int debugExcellentCount;
    [SerializeField, Header("デバッグ用のクリア回数(0-1)")]
    public int debugClearCount;
    [SerializeField, Header("デバッグ用の難易度(0-1)")]
    public int debugDifficulty;

    public static bool isDebugSwitch;
    public static bool isTitleReturnFlg;
    public static bool isKeyFlg;
    public static bool isHighscoreFlg;

    /// <summary>
    /// CharaSet.SetUpより呼ばれるデバッグモードの設定処理
    /// debugModeOn = falseなら呼ばれない
    /// </summary>
    public void DebugSetup() {
        isHighscoreFlg = isHighscore;
        isDebugSwitch = isStageDebugSwitch;
        isTitleReturnFlg = isRetrunFlg;
        isKeyFlg = isKeyboardFlg;
        if (isDebugSwitch) {
            // スイッチがオンならステージ数とエクセレント回数とクリア回数と難易度を代入
            GameData.instance.stage = debugStageNum;
            GameData.instance.excelentCount = debugExcellentCount;
            GameData.instance.gameClearCount = debugClearCount;
            GameData.instance.difficulty = debugDifficulty;
        }
        Debug.Log(isTitleReturnFlg);
        Debug.Log(isKeyFlg);
        Debug.Log(GameData.instance.stage);
        Debug.Log(debugExcellentCount);
    }

    /// <summary>
    /// 使用できるキャラの追加
    /// </summary>
    public void CharaDebugSetup() {
        GameData.instance.achievements[1] = charaFlg[0];
        GameData.instance.achievements[2] = charaFlg[1];
        GameData.instance.achievements[3] = charaFlg[2];
        GameData.instance.achievements[4] = charaFlg[3];
    }
}
