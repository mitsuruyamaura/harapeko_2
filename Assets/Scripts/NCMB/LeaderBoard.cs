using NCMB;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard {

    public int currentRank = 0;
    public List<NCMB.HighScore> topRankers;
    public List<NCMB.HighScore> neighbors = null;

    /// <summary>
    /// 現在のプレイヤーのハイスコアを取得してランクを取得
    /// 未使用
    /// </summary>
    /// <param name="currentScore"></param>
    public void FetchRank(float currentScore) {
        // データストアの「HighScore」クラスを検索
        NCMBQuery<NCMBObject> rankQuery = new NCMBQuery<NCMBObject>("HighScore");
        // 自分のハイスコアよりも大きいものを検索
        rankQuery.WhereGreaterThan("Score", currentScore);
        // 自分のハイスコアよりも大きいレコードの数を取得
        rankQuery.CountAsync((int count, NCMBException e) => {
            if(e != null) {
                // 件数取得失敗
                GameObject.FindGameObjectWithTag("LeaderBoardManager").GetComponent<LeaderBoardManager>().DipslayError();
            } else {
                // 取得成功。自分よりスコアが上の人がいたら自分はn+1位
                currentRank = count + 1;
            }
        });
    }

    /// <summary>
    /// サーバーからトップ15を取得して降順に並べる
    /// </summary>
    public void FetchTopRankers() {
        topRankers = new List<NCMB.HighScore>();
        // データストア「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
        // 結果を降順で並べる
        query.OrderByDescending("Score");
        // 取得の上限値
        query.Limit = 15;
        // 取得値を上限にforeachで取得
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if(e != null) {
                // 検索失敗時の処理
                GameObject.FindGameObjectWithTag("LeaderBoardManager").GetComponent<LeaderBoardManager>().DipslayError();
            } else {
                // 検索成功
                List<NCMB.HighScore> list = new List<NCMB.HighScore>();
                // 取得したレコードをHighScoreクラスとして保存
                foreach(NCMBObject obj in objList) {
                    float s = System.Convert.ToInt32(obj["Score"]);
                    string n = System.Convert.ToString(obj["Name"]);
                    Debug.Log(s);
                    Debug.Log(n);
                    list.Add(new HighScore(s,n));
                }
                // 上位15名を保存
                topRankers = list;
            }
        });
    }

    /// <summary>
    /// サーバーから自分のランク前後２件を取得
    /// 未使用
    /// </summary>
    /// <returns></returns>
    public void FetchNeighbors() {
        // HighScoreクラスを初期化する
        neighbors = new List<HighScore>();
        // スキップする数を決める（自分が1位が2位の時は調整する）
        int numSkip = currentRank - 3;
        if(numSkip < 0) {
            numSkip = 0;
        }
        // データストアの「HighScore」クラスから検索
        NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
        query.OrderByAscending("Score");
        // 取得開始の位置の取得と設定
        query.Skip = numSkip;
        query.Limit = 5;
        // 自分の周囲5名を上限に取得
        query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            if(e != null){
                // 検索失敗時の処理
            } else {
                // 検索成功時
                List<NCMB.HighScore> list = new List<HighScore>();
                // 取得したレコードをHighScoreクラスとして保存
                foreach(NCMBObject obj in objList) {
                    float score = System.Convert.ToInt32(obj["Score"]);
                    string name = System.Convert.ToString(obj["Name"]);
                    list.Add(new HighScore(score,name));
                }
                // 周囲5名を保存
                neighbors = list;
            }
        });
    }
}
