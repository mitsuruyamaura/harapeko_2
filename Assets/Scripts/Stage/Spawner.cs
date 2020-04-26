using UnityEngine;

public class Spawner : MonoBehaviour{

    [Header("生成するオブジェクトの指定")]
    public GameObject [] objects;          //  出現させる敵
    [Header("生成するまでの待機時間")]
    public float appearNextTime = 5f;
    [Header("生成する最大値")]
    public int maxNum = 3;

    private int objectsCount;             //  今何体の敵を出現させたか
    private float elapsedTime;            //  待ち時間計測フィールド
 
    void Start(){
        objectsCount = 0;
        elapsedTime = 0f;
    }

    private void Update(){
        if(Mathf.Approximately(Time.timeScale, 0f)) {
            //  Time.timeScaleが0の場合はアップデートを止めて操作を無効にしておく
            return;
        }
        //  この場所から出現する最大数を超えたら何もしない
        if (objectsCount >= maxNum){
            return;
        }

        elapsedTime += Time.deltaTime;
        
        if (elapsedTime > appearNextTime){            
            AppearObject();
            elapsedTime = 0f;
        }
    }

    /// <summary>
    /// オブジェクトを生成
    /// </summary>
    private void AppearObject(){
        var randomValue = Random.Range(0, objects.Length);

        //敵の向き(value=角度)をランダムに決定(RandomValueは0.0-1.0で取得なので360をかけて角度にする)
        //var randomRotatonY = Random.value * 180f;
        
        //  生成位置を調整するために取得
        Vector3 pos = transform.position;

        if(this.gameObject.tag == "Post" || this.gameObject.tag == "Construct") {
            //  生成物により生成位置を微調整
            pos.y += -0.2f;
            pos.z -= 2;
        } else {   // 敵
            pos.y -= 1;
            pos.z -= 2;
        }
    
        //  プレファブからインスタンス代入
        Instantiate(objects[randomValue], pos, transform.rotation);
       
        objectsCount++;
        elapsedTime = 0f;
    }
}