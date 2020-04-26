﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Score : MonoBehaviour {

    public Text scoreText;                  //  Text表示用
    public Text highScoreText;

    public static float score;              //  シーン間で引き継ぐため
    public static float highScore;　　　　　//　シーン間で引き継ぐため

    string highScoreKey = "highScore";      //  PlayerPrefsで保存するためのキー
    private float scoreBonus;               //  スコアへの修正
    public static bool isNewRecord;         //  ハイスコアが更新されたか判定。trueならハイスコアポップアップ表示処理オン

    public static float initScore;　　　　　// ステージ開始時のscore保存用
    public static float initHighScore;      // ステージ開始時のhighscore保存用
    public static bool isRetry;             // リトライ確定フラグ。RetryPopupのインステ広告再生後にtrueになる

    void Start() {
        Debug.Log(isRetry);
        // タイトルなら初期化する
        if(SceneManager.GetActiveScene().name == "Title") {
            Initialize();
        }
        // リトライならステージ開始時のスコアとハイスコアに戻す
        if (isRetry) {
            score = initScore;
            highScore = initHighScore;
        } else {
            // リトライでないなら、開始時のスコアとハイスコアを記録する
            initScore = score;
            initHighScore = highScore;
        }
        isRetry = false;
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }
	
    /// <summary>
    /// スコアボーナスの取得
    /// </summary>
    public void GetScoreBonus(float bonusPoint) {
        scoreBonus = bonusPoint;
        Debug.Log(scoreBonus);
    }

    /// <summary>
    /// ハイスコアとスコアの初期化処理
    /// </summary>
    public void Initialize(){
        score = 0;
        isNewRecord = false;
        //　ハイスコアを取得する。保存されていなければ20万を取得する
        highScore = PlayerPrefs.GetFloat(highScoreKey, 0);
        Debug.Log(highScore);
    }

    /// <summary>
    /// スコアの加算処理
    /// </summary>
    /// <param name="scorePoint"></param>
    public void AddPoint(float scorePoint){
        scorePoint *= scoreBonus;
        score += scorePoint;
        StartCoroutine(ScaleChangeScore(scorePoint));
        Debug.Log(scorePoint);
        if(highScore < score) {
            // ハイスコア更新確認。超えていれば数値を更新し、フラグを立てる
            if(!isNewRecord) {
                isNewRecord = true;
            }
            highScore = score;
            StartCoroutine(ScaleChangeHighScore());
            Debug.Log(highScore);
            Debug.Log(isNewRecord);
            if(highScore >= 200000 && !CharaSet.achievements[4]) {
                // witch5追加
                CharaSet.achievements[4] = true;
                CharaSet.SaveAchievement(4);
            }
        }
        //スコア・ハイスコアを表示する
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }

    /// <summary>
    /// Score更新時の数字の大きさの倍率の設定
    /// </summary>
    /// <param name="scorePoint"></param>
    /// <returns></returns>
    private float GetScale(float scorePoint) {
        float scale;
        if (0 < scorePoint && scorePoint <= 500) {
            scale = 1.15f;
        } else if (500 < scorePoint && scorePoint <= 1000) {
            scale = 1.3f;
        } else if (1000 < scorePoint && scorePoint <= 2000) {
            scale = 1.5f;
        } else {
            scale = 1.8f;
        }
        return scale;
    }

    /// <summary>
    /// Scoreのアニメ表示処理
    /// </summary>
    /// <param name="scorePoint"></param>
    /// <returns></returns>
    private IEnumerator ScaleChangeScore(float scorePoint) {
        float value = Random.Range(0.25f, 0.45f);
        float scale = GetScale(scorePoint);
        Debug.Log(scale);
        iTween.ScaleTo(scoreText.transform.gameObject, iTween.Hash("y", scale, "time", value));
        yield return new WaitForSeconds(value);
        iTween.ScaleTo(scoreText.transform.gameObject, iTween.Hash("y", 1.0f, "time", 0.1f));
    }

    /// <summary>
    /// HighScoreのアニメ表示処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScaleChangeHighScore() {
        float value = Random.Range(0.25f, 0.45f);
        iTween.ScaleTo(highScoreText.transform.gameObject, iTween.Hash("y", 1.2f, "time", value));
        yield return new WaitForSeconds(value);
        iTween.ScaleTo(highScoreText.transform.gameObject, iTween.Hash("y", 1.0f, "time", 0.1f));
    }

    /// <summary>
    /// ローカルのハイスコアの保存
    /// 各ステージクリア時とゲームオーバー時に処理
    /// </summary>
    public void Save(){
        PlayerPrefs.SetFloat(highScoreKey, highScore);
        Debug.Log(highScore);
        PlayerPrefs.Save();
        Debug.Log("Save");        
    }
}
