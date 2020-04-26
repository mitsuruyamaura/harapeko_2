using System.Collections;
using UnityEngine;

public class MoveFloor : MonoBehaviour {

    [Header("待機時間")]
    public float waitTime;
    [Header("X軸移動量")]
    public float verticalAdd;
    [Header("Y軸移動量")]
    public float horizontalAdd;
    [Header("移動させる時間")]
    public float moveTime;

    private void Start() {
        StartCoroutine(MoveTo());
    }

    IEnumerator MoveTo() {
        Vector3 pos = transform.position;
        iTween.MoveTo(gameObject, iTween.Hash("x", pos.x + verticalAdd, "y", pos.y + horizontalAdd, "time", moveTime));
        yield return new WaitForSeconds(moveTime - 2.5f);
        StartCoroutine(MoveFrom());
    }

    IEnumerator MoveFrom() {
        Vector3 pos = transform.position;
        iTween.MoveTo(gameObject, iTween.Hash("x", pos.x - verticalAdd, "y", pos.y - horizontalAdd, "time", moveTime));
        yield return new WaitForSeconds(moveTime - 2.5f);
        StartCoroutine(MoveTo());
    }
}
