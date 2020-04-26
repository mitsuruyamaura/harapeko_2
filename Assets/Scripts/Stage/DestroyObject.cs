using UnityEngine;

public class DestroyObject : MonoBehaviour {

    [Header("接触時に本オブジェクトを破壊させる、対象のタグ名")]
    public string targetName;

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == targetName) {
            Destroy(gameObject);
        }
    }
}
