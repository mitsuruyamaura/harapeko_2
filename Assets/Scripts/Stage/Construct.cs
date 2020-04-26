using UnityEngine;

public class Construct : MonoBehaviour{

    [Header("生成するオブジェクトの指定")]
    public GameObject [] items;

    void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Charge") {
            GameObject.FindGameObjectWithTag("Witch").GetComponent<WitchMove>().PlayHitEffect(transform.position);
            Destroy(gameObject);
            var randomValue = Random.Range(0, items.Length);
            //  アイテムをランダムで生成する
            Instantiate(items[randomValue], transform.position, transform.rotation);
        }
    }
}
