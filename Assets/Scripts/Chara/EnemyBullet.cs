using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    [SerializeField, HeaderAttribute("弾の攻撃力")]
    public float shotPower;

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Witch") {
            GameObject.FindGameObjectWithTag("HP").GetComponent<Life>().SubtractLife((int)shotPower);
            Destroy(gameObject);
        }
    }
}
