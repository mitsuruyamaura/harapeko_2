using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMail : MonoBehaviour {

    private int mailPoint;            //　このステージで食べ物と宝石を取った回数

    AudioSource sound1;               //  jewel獲得の音
    GameObject deliveryScore;         //  手紙配達(パワーアップアイテム生成)までの残り時間の表示用
    TextMesh deliveryText;

    int quotaPoint;                   //  パワーアップアイテム生成に必要な宝石と食べ物の合計ノルマ

    [SerializeField, HeaderAttribute("パワーアップアイテムの指定")]
    public GameObject [] Letters;　　 //  パワーアップアイテム生成用のインスタンス指定用

    [SerializeField, HeaderAttribute("Pアイテム出現用ノルマの下限値")]
    public int mimQuotaPoint;
    [SerializeField, HeaderAttribute("Pアイテム出現用ノルマの上限値")]
    public int maxQuotaPoint;

    void Start () {
        mailPoint = 0;

        //  ランダムでパワーアップアイテム生成までに必要なノルマを決定
        var randomQuota = Random.Range(mimQuotaPoint, maxQuotaPoint);
        quotaPoint = randomQuota;

        //  SE用
        sound1 = GetComponent<AudioSource>();

        //  チョコポストの上に表示するText
        deliveryScore = GameObject.Find("DeliveryScore");
        deliveryText = deliveryScore.GetComponent<TextMesh>();

        DisplayPost();
    }
	
    /// <summary>
    /// パワーアップアイテムの生成処理
    /// </summary>
    private void AppearLetter() {
        //  出現させる手紙（パワーアップアイテム）をランダムに選ぶ
        int randomLetter = Random.Range(0, Letters.Length);

        //  生成する位置を決定。スポナーを(0,0)としてみる
        Vector3 pos = transform.position;
        pos.y += 1.5f;

        //  もしも[0]ならZ軸を回転させて生成
        if(randomLetter == 0) {
            //  縦にして生成
            Quaternion rotation;
            float z = 90;
            rotation = Quaternion.Euler(0.0f, 0.0f, z);
            //  Letter1を生成する
            Instantiate(Letters[randomLetter], pos, rotation);
        } else {
            //  Letter2を生成する
            Instantiate(Letters[randomLetter], pos, transform.rotation);
        }
        //  カウントを戻す
        mailPoint = 0;

        //  次回のパワーアップアイテム生成までに必要なノルマを決定
        var randomPoint = Random.Range(mimQuotaPoint, maxQuotaPoint);
        quotaPoint = randomPoint;

        DisplayPost();
    }

    /// <summary>
    /// パワーアップアイテム出現ノルマの加算処理
    /// </summary>
    /// <param name="mailPoint"></param>
    /// <param name="isJewel"></param>
    public void AddMailPoint(int itemPoint, bool isJewel = false){
        //  食べ物を食べるか宝石を取ったらポイント増加
        mailPoint += itemPoint;
        if(isJewel) {
            //  宝石っぽいSEを鳴らす
            sound1.PlayOneShot(sound1.clip);
        }
        DisplayPost();

        if(mailPoint > (quotaPoint - 1)) {
            //  ノルマを超えたらパワーアップアイテム生成メソッドの呼出し
            AppearLetter();
        }
    }

    /// <summary>
    /// 手紙が配達される（パワーアップアイテム生成）までの残りカウント表示の更新
    /// </summary>
    private void DisplayPost() {
        deliveryText.text = (quotaPoint - mailPoint).ToString();
    }
}
