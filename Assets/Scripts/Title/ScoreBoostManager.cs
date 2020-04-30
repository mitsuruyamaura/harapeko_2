using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoostManager : MonoBehaviour {

    [SerializeField, Header("スコアブーストポップアップ")]
    public ScoreBoostPopup scoreBoostPopupPrefab;

    /// <summary>
    /// スコアブーストポップアップを生成する
    /// スコアブーストボタンより呼ばれる
    /// </summary>
	public void CreateScoreBurstPopup() {
        ScoreBoostPopup scoreBoostPopup =  Instantiate(scoreBoostPopupPrefab, GameObject.FindGameObjectWithTag("Canvas").transform, false);
        scoreBoostPopup.OpenPopUp();
    }
}
