using System.Collections;
using UnityEngine;

public class SwitchCollider : MonoBehaviour {

    [Header("アタッチされているBoxコライダーのオン/オフを切り替える")]
    public BoxCollider2D bCol;
    [Header("待機時間")]
    public float timer;

    void Start () {
        StartCoroutine(ActiveCollider(timer));
	}
	
    /// <summary>
    /// 指定時間後にコライダーをオンに切り替える
    /// </summary>
    /// <param name="timer"></param>
    /// <returns></returns>
	public IEnumerator ActiveCollider(float timer) {
        yield return new WaitForSeconds(timer);
        bCol.enabled = true;
    }
}
