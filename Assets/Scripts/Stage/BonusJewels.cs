using System.Collections;
using UnityEngine;

public class BonusJewels : MonoBehaviour {

    [Header("生成するオブジェクトの指定")]
    public GameObject[] objects;

    GameObject jewel;
    int bonusCount;

    /// <summary>
    /// エクセレントクリアによるボーナス宝石生成の有無の判定
    /// </summary>
    public void CheckExcellentBonus() {
        if(GameData.instance.stage < 7 && GameData.instance.excelentCount > 0) {
            // ボーナスステージ以外でエクセレントクリアなら
            CreateJewels();
        }
    }

    /// <summary>
    /// ボーナス宝石の生成
    /// </summary>
    public void CreateJewels() {
        bonusCount++;
        var randomValue = Random.Range(0, objects.Length);
        int randomPower = Random.Range(100, 300);
        float randomDirection = Random.Range(-0.7f, 0.7f);

        jewel = Instantiate(objects[randomValue], transform.position, transform.rotation);
        Rigidbody2D rb = jewel.GetComponent<Rigidbody2D>();
        rb.AddForce((Vector2.up + (Vector2.right * randomDirection)) * randomPower);
        // エクセレント回数だけ宝石を出す
        if(bonusCount < GameData.instance.excelentCount) {         
            StartCoroutine(ExcelentBonus());
        }
        Debug.Log(bonusCount);
    }

    /// <summary>
    /// エクセレント回数だけボーナス宝石の生成
    /// </summary>
    /// <returns></returns>
    IEnumerator ExcelentBonus() {
        yield return new WaitForSeconds(1.25f);
        CreateJewels();
    }
}
