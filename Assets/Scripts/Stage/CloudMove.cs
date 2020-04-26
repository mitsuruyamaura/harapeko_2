using UnityEngine;

public class CloudMove : MonoBehaviour {

    [SerializeField, Header("振幅幅")]
    public float amplitude = 0.01f;
    private int frameCount = 0;      //  フレームカウント

    private void FixedUpdate(){
        frameCount += 1;

        if (10000 <= frameCount){
            frameCount = 0;
        }

        if (0 == frameCount % 2){
            //  上下に振動させる（ふわふわ）
            float posYSin = Mathf.Sin(2.0f * Mathf.PI * (float)(frameCount % 200) / (200.0f - 1.0f));
            iTween.MoveAdd(gameObject, new Vector3(0, amplitude * posYSin, 0), 0.0f);
        }    
    }
}