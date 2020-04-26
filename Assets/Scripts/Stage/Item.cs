using UnityEngine;

public class Item : MonoBehaviour {

    [SerializeField,HeaderAttribute("HP回復量")]
    public int healPoint = 200;
    [SerializeField, HeaderAttribute("得点")]
    public float scorePoint = 100;
    [SerializeField, HeaderAttribute("パワーアップアイテム生成用のポイント")]
    public int itemPoint = 1;
    
    private enum ItemState {
        HEAL,
        INJURY,
        JEWEL
    }

    [SerializeField, HeaderAttribute("アイテム種類")]
    private ItemState itemState = ItemState.HEAL;

    private void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Witch") {                    //  キャラと衝突した時、HPタグの付いているオブジェクトのLifeスクリプトを取得する
            if(itemState != ItemState.JEWEL) {                 //  宝石以外ならライフ増減処理をいれる
                Life life = GameObject.FindGameObjectWithTag("HP").GetComponent<Life>();
                if(itemState == ItemState.HEAL) {                  //  アイテムの種類によってライフを減算するか加算するか分岐させる。どちらも引数はhealPoint
                    life.AddLife(healPoint);
                } else {
                    life.SubtractLife(healPoint);
                }
            }
            //  Scoreスクリプトに得点を引数にして渡す
            FindObjectOfType<Score>().AddPoint(scorePoint);
            if(itemState == ItemState.JEWEL) {
                //  GetMailスクリプトにパワーアップアイテム生成用のポイントを渡す。宝石の場合にはSEを鳴らすようにtrueで引数を渡す
                FindObjectOfType<GetMail>().AddMailPoint(itemPoint, true);
            } else {
                FindObjectOfType<GetMail>().AddMailPoint(itemPoint);
            }
            Destroy(gameObject);
        }
        if(col.gameObject.tag == "Bullet" || col.gameObject.tag == "Charge") {  //  負傷アイテムの場合、弾に接触したら自分自身を破壊する
            if(itemState == ItemState.INJURY) {
                Destroy(gameObject);
            }
        }
    }
}
