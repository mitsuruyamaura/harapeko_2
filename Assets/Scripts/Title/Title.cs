using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Title : MonoBehaviour
{
    [Header("タイトル用イメージ")]
    public SpriteRenderer[] titleImage;

    [Header("アニメさせるタイトルテキスト")]
    public Text txtTitle;
    public Button btnTitleAnime;

    private bool isAnime;             // アニメ再生中フラグ
    public float animeTime = 3.0f;    // アニメする時間

    void Start() {
        SoundManager.instance.PlayBgm(SoundManager.BGM_TYPE.TITLE);
        btnTitleAnime.onClick.AddListener(PreparationAnime);
        PreparationAnime();
    }

    /// <summary>
    /// タップ時にも呼ばれる
    /// アニメ再生の準備
    /// </summary>
    private void PreparationAnime() {
        if (isAnime) {
            return;
        }
        isAnime = true;
        // アニメ再生中でなければscaleをランダム範囲で設定
        StartCoroutine(PlayAnime());
    }

    /// <summary>
    /// アニメ再生
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAnime() {
        float randomValue = Random.Range(3.0f, 7.0f);
        Vector3 scale = new Vector3(randomValue, randomValue, randomValue);
        txtTitle.transform.DOShakeScale(animeTime);
        yield return new WaitForSeconds(0.6f);
        isAnime = false;
    }

    /// <summary>
    /// タイトル画面の設定処理。初期はどちらもfalse
    /// クリア状態でタイトル絵を変化させる
    /// ランキングとチュートリアルを閉じた際にも呼び出し
    /// </summary>
    public void DisplayTitleMode() {
        if (GameData.instance.gameClearCount == 0) {
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
