using UnityEngine;

public class RotateEffect : MonoBehaviour {

	void Update () {
        Vector3 currentX = Vector3.right * -5; //現在地
        Vector3 targetX = Vector3.right * 5;   //目標値

        float maxRadiansDelta = Time.time;    //2点間におけるラジアンの差分の範囲内で設定する
        float maxMagnitudeDelta = 0f;         //1回で何度回転させるか。

        transform.position = Vector3.RotateTowards(currentX, targetX, maxRadiansDelta, maxMagnitudeDelta);
    }
}
