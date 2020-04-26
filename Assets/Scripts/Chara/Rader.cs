using UnityEngine;

public class Rader : MonoBehaviour {

    private GameObject target;

    private void Start() {
        target = GameObject.FindGameObjectWithTag("Witch");
    }

    private void Update() {
        this.gameObject.transform.LookAt(target.transform.position);
    }

    //private void OnTriggerStay2D(Collider2D col) {
    //    if(col.gameObject.tag == "Witch") {
    //        //  rootを使うと最上位の親の情報を取得できる
    //        //  LookAtメソッドは指定した方向にオブジェクトの向きを回転させる
    //        transform.LookAt(target);
    //        Debug.Log("search");
    //    }
    //}
}
