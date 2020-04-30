using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;

public class Score : MonoBehaviour {

    // Text表示用
    public Text scoreText;
    public Text highScoreText;

    string highScoreKey = "highScore";      //  PlayerPrefsで保存するためのキー
    private float scoreBonus;               //  スコアへの修正
    
    void Start() {
        Debug.Log(GameData.instance.isRetry);
        // タイトルなら初期化する
        if(SceneManager.GetActiveScene().name == "Title") {
            Initialize();
        }
        // リトライならステージ開始時のスコアとハイスコアに戻す
        if (GameData.instance.isRetry) {
            GameData.instance.score = GameData.instance.initScore;
            GameData.instance.highScore = GameData.instance.initHighScore;
        } else {
            // リトライでないなら、開始時のスコアとハイスコアを記録する
            GameData.instance.initScore = GameData.instance.score;
            GameData.instance.initHighScore = GameData.instance.highScore;
        }
        GameData.instance.isRetry = false;
        scoreText.text = GameData.instance.score.ToString();
        highScoreText.text = GameData.instance.highScore.ToString();
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
        GameData.instance.score = 0;
        GameData.instance.isNewRecord = false;
        //　ハイスコアを取得する。保存されていなければ20万を取得する
        GameData.instance.highScore = PlayerPrefs.GetFloat(highScoreKey, 0);
        Debug.Log(GameData.instance.highScore);
    }

    /// <summary>
    /// スコアの加算処理
    /// </summary>
    /// <param name="scorePoint"></param>
    public void AddPoint(float scorePoint){
        scorePoint *= scoreBonus;
        GameData.instance.score += scorePoint;
        StartCoroutine(ScaleChangeScore(scorePoint));
        Debug.Log(scorePoint);
        if(GameData.instance.highScore < GameData.instance.score) {
            // ハイスコア更新確認。超えていれば数値を更新し、フラグを立てる
            if(!GameData.instance.isNewRecord) {
                GameData.instance.isNewRecord = true;
            }
            GameData.instance.highScore = GameData.instance.score;
            StartCoroutine(ScaleChangeHighScore());
            Debug.Log(GameData.instance.highScore);
            Debug.Log(GameData.instance.isNewRecord);
            if(GameData.instance.highScore >= 200000 && !GameData.instance.achievements[4]) {
                // witch5追加
                GameData.instance.achievements[4] = true;
                CharaSet.SaveAchievement(4);
            }
        }
        //スコア・ハイスコアを表示する
        scoreText.text = GameData.instance.score.ToString();
        highScoreText.text = GameData.instance.highScore.ToString();
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
        //float scale = GetScale(scorePoint);
        //Debug.Log(scale);
        Sequence seq = DOTween.Sequence();
        seq.Append(scoreText.transform.DOScale(GetScale(scorePoint), value));
        yield return new WaitForSeconds(value);
        seq.Append(scoreText.transform.DOScale(1.0f, 0.1f));
        //iTween.ScaleTo(scoreText.transform.gameObject, iTween.Hash("y", scale, "time", value));
        //yield return new WaitForSeconds(value);
        //iTween.ScaleTo(scoreText.transform.gameObject, iTween.Hash("y", 1.0f, "time", 0.1f));
    }

    /// <summary>
    /// HighScoreのアニメ表示処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ScaleChangeHighScore() {
        float value = Random.Range(0.25f, 0.45f);
        Sequence seq = DOTween.Sequence();
        seq.Append(highScoreText.transform.DOScale(1.2f, value));
        yield return new WaitForSeconds(value);
        seq.Append(highScoreText.transform.DOScale(1.0f, 0.1f));
        //iTween.ScaleTo(highScoreText.transform.gameObject, iTween.Hash("y", 1.2f, "time", value));
        //yield return new WaitForSeconds(value);
        //iTween.ScaleTo(highScoreText.transform.gameObject, iTween.Hash("y", 1.0f, "time", 0.1f));
    }

    /// <summary>
    /// ローカルのハイスコアの保存
    /// 各ステージクリア時とゲームオーバー時に処理
    /// </summary>
    public void Save(){
        PlayerPrefs.SetFloat(highScoreKey, GameData.instance.highScore);
        Debug.Log(GameData.instance.highScore);
        PlayerPrefs.Save();
        Debug.Log("Save");        
    }
}
