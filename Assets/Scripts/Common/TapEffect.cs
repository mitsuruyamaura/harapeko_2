using UnityEngine;

public class TapEffect : MonoBehaviour
{
    public Camera _camera;
    [Header("タッチした時のエフェクト")]
    public ParticleSystem onClick;
    [Header("スライドした時のエフェクト")]
    public ParticleSystem onSlide;
    
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Update() {        
        if (Input.GetMouseButtonDown(0)) {          
            onClick.transform.position = _camera.ScreenToWorldPoint(Input.mousePosition + _camera.transform.forward * 10);
            onClick.Emit(1);
            onSlide.Play();
        }
        if (Input.GetMouseButton(0)) {
            // パーティクルをタッチスライドに追従させる
            onSlide.transform.position = _camera.ScreenToWorldPoint(Input.mousePosition + _camera.transform.forward * 10);
        }
        if (Input.GetMouseButtonUp(0)) {
            onSlide.Stop();
        }
    }
}
