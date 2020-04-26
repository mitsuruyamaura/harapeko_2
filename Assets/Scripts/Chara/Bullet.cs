using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private GameObject player;
    private WitchMove witchMove;

    [SerializeField, HeaderAttribute("弾の速度")]
    public float speed = 7f;
    [SerializeField, HeaderAttribute("弾の攻撃力")]
    public int power = 1;

    void Start() {
        player = GameObject.FindWithTag("Witch");
        witchMove = player.GetComponent<WitchMove>();

        //  rigiddoby2Dコンポーネントを取得
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //　キャラの向いている方向に弾を飛ばす
        rb.velocity = new Vector3(-speed * player.transform.localScale.x, rb.velocity.y, 0);

        //　画像の向きをキャラに合わせる
        Vector3 temp = transform.localScale;
        temp.x = player.transform.localScale.x;
        transform.localScale = temp;

        if(witchMove.witchState == WitchMove.WitchState.POWERFUL) {
            //  パワフル状態とその他で分岐
            Destroy(gameObject, 2.0f);
        } else {
            Destroy(gameObject, 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(witchMove.witchState != WitchMove.WitchState.POWERFUL) {
            //  パワフル以外の状態時にEnemyタグかconstructタグの対象にバレットが接触した場合、破壊する
            if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Construct") {
                Destroy(gameObject);
            }
        }
    }
}
