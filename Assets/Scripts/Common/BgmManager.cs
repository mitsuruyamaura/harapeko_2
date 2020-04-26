using UnityEngine;

public class BgmManager : MonoBehaviour {

    public static BgmManager instance;

    [Header("ステージのBGM")]
    public AudioSource stageBGM;         //  各ステージごとのBGM。各ステージで変える
    [Header("ステージクリアSE")]
    public AudioSource clearBGM;
    [Header("HP減少SE")]
    public AudioSource lifeDownSE;
    [Header("HP回復SE")]
    public AudioSource lifeRecoverSE;
    [Header("ゲームオーバーSE")]
    public AudioSource gameOverSE;

    public AudioClip[] bgms;
    public AudioClip[] ses;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    void Start () {
        stageBGM.Play();
	}

    /// <summary>
    /// ゲームオーバー時のBGM処理
    /// </summary>
    public void StopBgm() {
        stageBGM.Stop();
        gameOverSE.PlayOneShot(gameOverSE.clip);
    }

    /// <summary>
    /// ステージクリア時のBGM処理
    /// </summary>
    public void GameClear(){
        stageBGM.Stop();
        if (!clearBGM.isPlaying){
            clearBGM.PlayOneShot(clearBGM.clip);
        }
    }

    /// <summary>
    /// ライフ回復SE再生
    /// </summary>
    public void RecoverLifeSE() {
        lifeRecoverSE.PlayOneShot(lifeRecoverSE.clip);
    }

    /// <summary>
    /// ライフ減少SE再生
    /// </summary>
    public void DamageLifeSE() {
        lifeDownSE.PlayOneShot(lifeDownSE.clip);
    }

    /// <summary>
    /// 敵の被ダメージ時のSE再生
    /// </summary>
    //public void EnemyDamageSePlay(Vector2 pos) {
    //    AudioSource.PlayClipAtPoint(enemyDamagedSe, pos);
    //    Debug.Log("damage");
    //}
}
