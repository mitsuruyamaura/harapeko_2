using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultDisplayManager : MonoBehaviour {

    [Header("メインキャンバス")]
    public GameObject canvas;
    [Header("ゲームオーバー表示_1")]
    public Text gameOverText;
    [Header("ゲームクリア表示_1")]
    public Text gameClearText;
    [Header("ゲームクリア表示_2")]
    public Text gameClear1Text;

    /// <summary>
    /// ステージクリア時のリザルト表示処理
    /// </summary>
    public void DisplayClearText(bool isExcellent) {
        // ステージクリアの文字を画面に表示
        gameClearText.enabled = true;
        gameClear1Text.enabled = true;
        // ボーナスステージなら
        if (SceneManager.GetActiveScene().name == StageType.Bonus_Stage.ToString()) {
            switch (GameData.instance.language) {
                case 0:
                    gameClearText.text = "こんぐらっちゅ★れーしょん";
                    break;
                case 1:
                    gameClearText.text = "Congratulations!!";
                    break;
                case 2:
                    gameClearText.text = "祝贺清算!!";
                    break;
            }
        } else {
            // それ以外のステージならクリア状態で分岐
            if (isExcellent) {
                switch (GameData.instance.language) {
                    case 0:
                        gameClearText.text = "♪えくせれんとっ♪";
                        break;
                    case 1:
                        gameClearText.text = "♪EXCELLENT♪"; 
                        break;
                    case 2:
                        gameClearText.text = "♪优秀♪";
                        break;
                }
            } else {
                switch (GameData.instance.language) {
                    case 0:
                        gameClearText.text = "★ステージ くりあー★";
                        break;
                    case 1:
                        gameClearText.text = "♪STAGE CLEAR♪";
                        break;
                    case 2:
                        gameClearText.text = "♪阶段清晰♪";
                        break;
                }
            }
        }
        switch (GameData.instance.language) {
            case 0:
                JapaneseDisplayMessage();
                break;
            case 1:
                EnglishDisplayMessage();
                break;
            case 2:
                ChineseDisplayMessage();
                break;
        }
    }

    /// <summary>
    /// 日本語のステージクリアメッセージ
    /// </summary>
    private void JapaneseDisplayMessage() {
        // 各ステージの最終シーンかボーナスステージがそれ以外かで分岐
        if (SceneManager.GetActiveScene().name == "Stage_5") {
            gameClear1Text.text = "のーまる コース クリア おめでとう!!\n10 秒後 にボーナス ステージに進みます";
        } else if (SceneManager.GetActiveScene().name == "Stage_10") {
            gameClear1Text.text = "はーど コース クリア おめでとう!!\n10 秒後 にボーナス ステージに進みます";
        } else if (SceneManager.GetActiveScene().name == "Bonus_Stage" && GameData.instance.gameClearCount == 0) {
            gameClear1Text.text = "はーど コースが 開放されました!!\n10 秒後 にタイトルに戻ります";
        } else if (SceneManager.GetActiveScene().name == "Bonus_Stage" && GameData.instance.gameClearCount != 0) {
            gameClear1Text.text = "あそんでくれて ありがとう!!\nハイスコア ランキングに チャレンジしてね!!";
        } else {
            gameClear1Text.text = "10 秒後 に次のステージに進みます";
            if (SceneManager.GetActiveScene().name == "Stage_1") {
                gameClear1Text.text += "\nできるだけアイテムを拾っちゃおう!!";
            }
        }
    }

    /// <summary>
    /// 英語のステージクリアメッセージ
    /// </summary>
    private void EnglishDisplayMessage() {
        // 各ステージの最終シーンかボーナスステージがそれ以外かで分岐
        if (SceneManager.GetActiveScene().name == "Stage_5") {
            gameClear1Text.text = "Congratulations on clearing normal course!!\nGo to the bonus stage after 10 seconds.";
        } else if (SceneManager.GetActiveScene().name == "Stage_10") {
            gameClear1Text.text = "Congratulations on clearing hard course!!\nGo to the bonus stage after 10 seconds.";
        } else if (SceneManager.GetActiveScene().name == "Bonus_Stage" && GameData.instance.gameClearCount == 0) {
            gameClear1Text.text = "Hard course has been released!!\nReturn to title after 10 seconds.";
        } else if (SceneManager.GetActiveScene().name == "Bonus_Stage" && GameData.instance.gameClearCount != 0) {
            gameClear1Text.text = "Thank you for playing!!\nChallenge the high score ranking!!";
        } else {
            gameClear1Text.text = "Go to the next stage after 10 seconds.";
            if (SceneManager.GetActiveScene().name == "Stage_1") {
                gameClear1Text.text += "\nLet's pick up items as much as possible!!";
            }
        }
    }

    /// <summary>
    /// 中文(簡体)のステージクリアメッセージ
    /// </summary>
    private void ChineseDisplayMessage() {
        // 各ステージの最終シーンかボーナスステージがそれ以外かで分岐
        if (SceneManager.GetActiveScene().name == "Stage_5") {
            gameClear1Text.text = "恭喜您清除正常课程!!\n在10秒内进入奖励阶段";
        } else if (SceneManager.GetActiveScene().name == "Stage_10") {
            gameClear1Text.text = "恭喜您完成了艰难的课程!!\n在10秒内进入奖励阶段";
        } else if (SceneManager.GetActiveScene().name == "Bonus_Stage" && GameData.instance.gameClearCount == 0) {
            gameClear1Text.text = "艰难的课程已经发布!!\n10秒后返回标题";
        } else if (SceneManager.GetActiveScene().name == "Bonus_Stage" && GameData.instance.gameClearCount != 0) {
            gameClear1Text.text = "谢谢你的参与!!\n挑战高分排名!!";
        } else {
            gameClear1Text.text = "10秒后进入下一阶段";
            if (SceneManager.GetActiveScene().name == "Stage_1") {
                gameClear1Text.text += "\n让我们尽可能地拿起物品!!";
            }
        }
    }

    /// <summary>
    /// ゲームオーバー時のリザルト表示
    /// </summary>
    public void DisplayGameOverText() {
        gameOverText.enabled = true;
        switch (GameData.instance.language) {
            case 0:
                gameClear1Text.text = "げーむ おーばぁ…";
                break;
            case 1:
                gameClear1Text.text = "Game Over...";
                break;
            case 2:
                gameClear1Text.text = "游戏结束...";
                break;
        }
    }

    /// <summary>
    /// リザルト表示を非表示にする処理
    /// </summary>
    public void HiddenResultText() {
        // 画面のリザルト表示を非表示にする
        canvas.SetActive(false);
    }
}
