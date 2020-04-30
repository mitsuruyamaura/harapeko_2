using UnityEngine;
using UnityEngine.UI;

public class ViewHighScore : MonoBehaviour {

    [Header("ハイスコア表示用")]
    public Text viewHighScoreText;

    /// <summary>
    /// 最終のハイスコアを表示
    /// ポップアップ表示時に呼び出し
    /// </summary>
    public void DipslayHighScore() {
        // カンマをつけて表示
        viewHighScoreText.text = GameData.instance.highScore.ToString("N0");
    }	
}
