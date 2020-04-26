using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// クレジット名をCregidDatasから取得・表示し、風船を移動させる
/// </summary>
public class FitCredit : MonoBehaviour {

    public Credit credit;
    public TextMesh creditTxt;
    [SerializeField, Header("クレジットNo")]
    private int creditNo;
    Rigidbody2D rb;
    Vector3 pos;
    
    void Start() {
        pos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        // クレジットNoを敵を倒した数から取得
        creditNo = GameObject.FindGameObjectWithTag("HP").GetComponent<GameClearController>().destroyCount;
        SetupAndDisplayCredit();
    }

    /// <summary>
    /// クレジットの内容を設定して表示する
    /// </summary>
    private void SetupAndDisplayCredit() {
        // CreditDatasからcreditNoと同じnoを持つDataを探す
        foreach (Credit.CreditData data in credit.creditDatas) {
            if (data.no == creditNo) {
                creditTxt.text = data.companyName + "\n";
                creditTxt.text += data.assetName;
            }
        }
    }

    /// <summary>
    /// 風船のように左右に揺らしながら上昇させる
    /// </summary>
    void FixedUpdate() {
        pos.y += 0.01f;
        rb.MovePosition(new Vector3 (pos.x + Mathf.PingPong(Time.time,2),pos.y,pos.z));
    }
}
