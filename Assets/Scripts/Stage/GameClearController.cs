using UnityEngine;
using UnityEngine.UI;

public class GameClearController : MonoBehaviour {

    [Header("目標討伐数の表示テキスト")]
    public Text counterText;
    [Header("ステージクリアの目標討伐数")]
    public int clearCount = 4;
    public int destroyCount;      // 敵の討伐数
    private int resaltCount;  　　 // ステージクリアまでの敵の数

    private AudioSource sound1;    // 敵を倒した際のSE

    public FireWorks[] fireworks;

    void Start() {
        // 敵を倒す数を設定。画面にカウントダウン表示用
        resaltCount = clearCount;
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sound1 = audioSources[0];
        DisplayResultText();
    }
    
    /// <summary>
    /// クリア目標数をライフの下に表示する処理
    /// TODO 後々はチュートリアルが終了していれば表示
    /// </summary>
    public void DisplayResultText() {
        // 満腹ゲージ下部にクリアまでに必要な敵のカウント数を常時更新して表示
        switch (GameData.instance.language){
            case 0:         
            counterText.text = "あと " + resaltCount + " ひき やっつければクリア";
                break;
            case 1:
                counterText.text = "Destroy " + resaltCount + " enemies to clear this stage";
                break;
            case 2:
                counterText.text = "消灭 " + resaltCount + " 个敌人以清除这个阶段";
                break;
        }
    }

    /// <summary>
    /// 敵の討伐数と残数の管理
    /// </summary>
    /// <param name="EnemyPoint"></param>
    public void AddDestroyPoint(int enemyPoint) {
        if(destroyCount < clearCount) {
            // 目標数に達していないなら敵を倒したら討伐数プラス
            destroyCount += enemyPoint;
            sound1.PlayOneShot(sound1.clip);
            // 敵の残数を減らす
            resaltCount -= enemyPoint;
            CheckCleardStage();
        }
    }

    /// <summary>
    /// ステージクリア判定と討伐数更新処理
    /// </summary>
    private void CheckCleardStage() {
        // 倒した敵の数がクリア数値以下ならクリア目標の表示数を更新
        if(resaltCount < clearCount) {
            DisplayResultText();
        }
        if(destroyCount >= clearCount) {
            counterText.text = " ";
            // 倒した敵の数が規定数を超えたら
            ClearStage();
        }
    }

    /// <summary>
    /// ステージクリア処理
    /// </summary>
    private void ClearStage() {
        //ライフの減少をストップ。クリアの状態を更新する。(エクセレント回数の更新)
        GameObject.FindGameObjectWithTag("HP").GetComponent<Life>().Clear(); ;        
        // BGMを止めてクリアSEを流す
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(SoundManager.SE_TYPE.STAGE_CLAER);
        // スコアアタックモードでないなら次のステージへ遷移させる処理を呼ぶ
        GameObject.Find("ScoreManager").GetComponent<StageManager>().NextStage(1);
        for (int i = 0; i < fireworks.Length; i++) {
            fireworks[i].SetOffFireWorks(Random.Range(0.5f, 1.5f));
        }
        // ３か所で花火を打ち上げて、エクセレントクリアなら同じ位置でボーナス宝石を打ち上げる
        //FireWorks fireWorks = GameObject.Find("FireWorks").GetComponent<FireWorks>();
        //FireWorks fireWorks1 = GameObject.Find("FireWorks1").GetComponent<FireWorks>();
        //FireWorks fireWorks2 = GameObject.Find("FireWorks2").GetComponent<FireWorks>();        
        //var randomWait = Random.Range(0.5f, 1.5f);
        //var randomWait1 = Random.Range(0.5f, 1.5f);
        //var randomWait2 = Random.Range(0.5f, 1.5f);
        //fireWorks.SetOffFireWorks(randomWait);
        //fireWorks1.SetOffFireWorks(randomWait1);
        //fireWorks2.SetOffFireWorks(randomWait2);
    }
}
