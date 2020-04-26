using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル画面用の浮遊する雲クラス
/// </summary>
public class RandomCloudMove : MonoBehaviour {

    const float minAmplitude = -0.05f;
    const float maxAmplitude = 0.05f;
    const float minTime = 20.0f;
    const float maxTime = 30.0f;

    [SerializeField, Range(minAmplitude, maxAmplitude), Header("横方向の振幅幅")]
    public float amplitudeX;
    [SerializeField, Range(minAmplitude, maxAmplitude), Header("縦方向の振幅幅")]
    public float amplitudeY;
    [SerializeField, Range(minTime, maxTime), Header("振幅幅ランダム設定までの待機時間")]
    public float changeTime;

    private int frameCount = 0;      // フレームカウント
    private float timer;             // ランダム設定時間のカウント用

    private void Start() {
        RandomSetDistance();
    }

    /// <summary>
    /// 縦横方向の振幅幅をランダムで変える
    /// </summary>
    private void RandomSetDistance() {
        changeTime = Random.Range(minTime,maxTime);
        amplitudeX = Random.Range(minAmplitude,maxAmplitude);
        amplitudeY = Random.Range(minAmplitude, maxAmplitude);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > changeTime) {
            timer = 0;
            RandomSetDistance();
        }    
    }

    private void FixedUpdate() {
        frameCount += 1;

        if (10000 <= frameCount) {
            frameCount = 0;
        }

        if (0 == frameCount % 2) {
            //  上下に振動させる（ふわふわ）
            float posYSin = Mathf.Sin(2.0f * Mathf.PI * (float)(frameCount % 200) / (200.0f - 1.0f));
            iTween.MoveAdd(gameObject, new Vector3(0, amplitudeY * posYSin, 0), 0.0f);
        }
        if (0 == frameCount % 2) {
            //  左右に振動させる（ふわふわ）
            float posXSin = Mathf.Sin(2.0f * Mathf.PI * (float)(frameCount % 200) / (200.0f - 1.0f));
            iTween.MoveAdd(gameObject, new Vector3(amplitudeX * posXSin, 0, 0), 0.0f);
        }
    }
}
