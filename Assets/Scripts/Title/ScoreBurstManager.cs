using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBurstManager : MonoBehaviour {

    [SerializeField, Header("スコアブーストポップアップ")]
    public ScoreBurstPopup scoreBurstPopupPrefab;

    /// <summary>
    /// スコアブーストポップアップを生成する
    /// スコアブーストボタンより呼ばれる
    /// </summary>
	public void CreateScoreBurstPopup() {
        ScoreBurstPopup scoreBurstPopup =  Instantiate(scoreBurstPopupPrefab, GameObject.FindGameObjectWithTag("Canvas").transform, false);
        scoreBurstPopup.OpenPopUp();
    }
}
