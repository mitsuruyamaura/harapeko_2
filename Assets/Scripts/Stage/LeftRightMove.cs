using UnityEngine;

public class LeftRightMove : MonoBehaviour {
    
    public float amplitude = 0.01f;  //  振幅幅
    private int frameCount = 0;      //  フレームカウント

    private void FixedUpdate(){
        frameCount += 1;

        if (10000 <= frameCount){
            frameCount = 0;
        }
        if (0 == frameCount % 2){
            //  左右に振動させる（ふわふわ）
            float posXSin = Mathf.Sin(2.0f * Mathf.PI * (float)(frameCount % 200) / (200.0f - 1.0f));
            iTween.MoveAdd(gameObject, new Vector3(amplitude * posXSin,0, 0), 0.0f);
        }
    }
}
