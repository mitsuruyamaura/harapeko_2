using UnityEngine;

public class RandomEffectPositon : MonoBehaviour {

	void Start () {
        transform.position = new Vector3(Random.Range(-7, 7), Random.Range(-5, 3), 0);
    }
}
