﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleTextAnimetion : MonoBehaviour {

    [Header("アニメさせるテキスト")]
    public Text txtTitle;
    public Button btnTitle;

    private bool isAnime;      // アニメ再生中フラグ
    public float animeTime = 3.0f;    // アニメする時間

    void Start() {
        btnTitle.onClick.AddListener(PreparationAnime);
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


}