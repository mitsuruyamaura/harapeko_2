using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleImageController : MonoBehaviour {

    [SerializeField, HeaderAttribute("タイトル絵")]
    public SpriteRenderer[] titleImage;

    /// <summary>
    /// タイトル画面の設定処理。初期はどちらもfalse
    /// クリア状態でタイトル絵を変化させる
    /// ランキングとチュートリアルを閉じた際にも呼び出し
    /// </summary>
    public void DisplayTitleMode() {
        if(StageManager.gameClearCount == 0) {
            titleImage[0].enabled = true;
        } else {
            // ノーマルステージ　クリア後
            titleImage[1].enabled = true;
        }
    }

    /// <summary>
    /// ランキング表示とチュートリアル表示時の背景画の表示処理
    /// </summary>
    public void DisplayChangeImage() {
        titleImage[0].enabled = true;
    }
}
