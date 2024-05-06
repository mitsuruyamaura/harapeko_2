using System.Collections.Generic;
using System.Collections;
using UnityEngine;
//using NCMB;

namespace NCMB {
    public class HighScore {
        public float score{ get; set; }
        public string name{ get; private set; }

        // コンストラクタ
        public HighScore(float _score, string _name) {
            score = _score;
            name = _name;
        }

        /// <summary>
        /// サーバーにハイスコアを保存
        /// セーブの可否を戻す。true=セーブ完了。
        /// </summary>
        public IEnumerator Save(HighScore high) {
            //NCMBObject obj = new NCMBObject("HighScore");
            //obj["Score"] = high.score;
            //obj["Name"] = high.name;
            //Debug.Log(high.score);
            //Debug.Log(high.name);
            //bool isSaved = false;
            //obj.SaveAsync((NCMBException e) => {              
            //    if(e == null){
            //        // 成功時の処理
            //        Debug.Log("サーバーセーブに成功しました！");
            //        isSaved = true;
            //        Debug.Log(isSaved);
            //    } else {
            //        // エラー時の処理
            //        Debug.Log("サーバーに接続できません。サーバーセーブに失敗しました。");
            //    }
            //});
            yield return new WaitForSeconds(1.2f);
            //Debug.Log(isSaved);
            //yield return isSaved;
            // ログインユーザーを取得してサーバーに書き込む方法。未使用
            // ニフクラのデータストアの「Ranking」からnameをキーにして検索
            //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            //query.WhereEqualTo("Name", name);
            //query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            //「HighScore」クラスに書き込む
            //NCMBObject obj = new NCMBObject("HighScore");

            //if(e == null) {
            // 検索に成功したら
            //obj["Score"] = score;
            //obj["Name"] = name;
            //Debug.Log(name);
            //obj.SaveAsync();
            //});
            //Debug.Log("サーバーセーブ");
        }

        /// <summary>
        /// サーバーからログインユーザー名を元にハイスコアを取得
        /// 未使用
        /// </summary>
        public void Fetch() {
            // ニフクラのデータストアの「Ranking」からnameをキーにして検索
            //NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("HighScore");
            //query.WhereEqualTo("Name", name);
            //query.FindAsync((List<NCMBObject> objList, NCMBException e) => {
            //    Debug.Log(e);
            //    if(e == null) {
            //        if(objList.Count == 0) {
            //            // 検索に成功してハイスコアが未登録なら
            //            NCMBObject obj = new NCMBObject("HighScore");
            //            obj["Name"] = name;
            //            obj["Score"] = 0;
            //            obj.SaveAsync();
            //            score = 0;
            //        } else {
            //            // ハイスコアが登録済なら 32 ビット符号付き整数に変換
            //            score = System.Convert.ToInt32(objList[0]["Score"]);
            //        }
            //    }
            //});
        }

        /// <summary>
        /// ランキングで表示するための文字列を整形して戻す
        /// 未使用
        /// </summary>
        /// <returns></returns>
        public string Print() {
            return name + ' ' + score;
        }

        /// <summary>
        /// ランキングに表示する名前を整形して戻す
        /// </summary>
        /// <returns></returns>
        public string PrintName() {
            return name;
        }

        /// <summary>
        /// ランキングに表示するスコアを整形して戻す
        /// </summary>
        /// <returns></returns>
        public string PrintScore() {
            return score.ToString();
        }
    }
}
