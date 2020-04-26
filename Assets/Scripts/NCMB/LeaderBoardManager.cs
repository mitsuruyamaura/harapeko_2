using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class LeaderBoardManager : MonoBehaviour {

    private LeaderBoard leaderBoard;
    private List<NCMB.HighScore> currentHighScore;

    [Header("ランキングポップアップ")]
    public GameObject rankingPopup;
    [Header("バックグラウンド表示")]
    public GameObject backGroundPanel;
    [Header("ステートメッセージ表示用(エラー/処理中)")]
    public Text stateText;
    [Header("トップ15名前表示用")]
    public Text[] topName;
    [Header("トップ15スコア表示用")]
    public Text[] topScore;
    [Header("画面遷移用フェイドアウト／イン制御")]
    public FadeManager fadeManager;

    //private bool isScoreFetched;
    //private bool isRankFetched;
    //private bool isLeaderBoardFetched;

    /// <summary>
    /// ランキングボタンを押したらポップアップ表示処理
    /// </summary>
    public void OpenRankingPopUp() {
        fadeManager.TransitionTutorial();
        rankingPopup.SetActive(true);
        backGroundPanel.SetActive(false);
        leaderBoard = new LeaderBoard();
        Debug.Log("ランキング初期化");
        // 現在のハイスコアを取得
        //string name = FindObjectOfType<UserAuth>().CurrentPlayer();
        //currentHighScore = new List<NCMB.HighScore>();
        //currentHighScore.Fetch();

        //GetHighScore();
        GetRanking();
        StartCoroutine(SetRanking());
    }

    /// <summary>
    /// 現在のプレイヤーのハイスコアを取得
    /// </summary>
    //private void GetHighScore() {
    //    // 現在のハイスコアの取得が完了したら一度だけ実行
    //    if(currentHighScore.score != -1 && !isScoreFetched) {
    //        leaderBoard.FetchRank(currentHighScore.score);
    //        isScoreFetched = true;
    //    }
    //}

    /// <summary>
    /// トップ15位までのハイスコアを取得
    /// </summary>
    private void GetRanking() {
        // 現在の順位の取得が完了したら一度だけ実行
        // 上位5名と周囲5名のランキングを取得
        //if(leaderBoard.currentRank != 0 && isRankFetched) {
        leaderBoard.FetchTopRankers();
        //leaderBoard.FetchNeighbors();
        //isRankFetched = true;
        //currentHighScore = leaderBoard.FetchTopRankers();
        //}
    }

    /// <summary>
    /// トップ15位までのハイスコアをランキングで表示
    /// </summary>
    private IEnumerator SetRanking() {
        // ステートテキストに処理中表示を出す       
        yield return new WaitForSeconds(0.8f);
        stateText.enabled = true;
        stateText.text = "処理中です…";
        yield return new WaitForSeconds(0.7f);
        stateText.enabled = false;
        backGroundPanel.SetActive(true);
        // ランキングの取得が完了したら一度だけ実行
        //if(leaderBoard.topRankers != null && leaderBoard.neighbors != null && !isLeaderBoardFetched) {
        //    // 自分が1位と2位のときだけ順位表示を調整
        //    int offset = 2;
        //    if(leaderBoard.currentRank == 1) {
        //        offset = 0;
        //    }
        //    if(leaderBoard.currentRank == 2) {
        //        offset = 1;
        //    }
        // 取得したトップ15ランキングを表示
        for(int i = 0; i < leaderBoard.topRankers.Count; i++) {
            topName[i].text = leaderBoard.topRankers[i].PrintName();
            topScore[i].text = leaderBoard.topRankers[i].PrintScore();
        }
        // 取得したライバルランキングを表示
        //for(int i = 0; i < leaderBoard.neighbors.Count; i++) {
        //    neighbor[i].text = leaderBoard.currentRank - offset + i + "." + leaderBoard.neighbors[i].Print();
        //}
        //isLeaderBoardFetched = true;
        //}
    }

    /// <summary>
    /// エラー時の処理
    /// </summary>
    public void DipslayError() {
        stateText.enabled = true;
        stateText.text = "サーバーからの\nデータの取得に失敗しました…";
    }

    /// <summary>
    /// ポップアップを閉じタイトル画面へ遷移する処理
    /// </summary>
    public void CloseRankingPopUp() {
        rankingPopup.SetActive(false);
        fadeManager.TransitionTitle(0.7f);
    }
}
