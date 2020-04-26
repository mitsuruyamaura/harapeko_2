using System.Collections;
using UnityEngine;

public class Fireboll : MonoBehaviour {

    private float shotIntarval;        //  ショットするまでの待機時間。ランダム値
    private float shotTimer;      　　 //  弾をチャージする時間のカウント用
    private int shotCount;
    public GameObject shooter;           //  弾を発射する敵オブジェクト
    private GameObject enemyShell;     //  インスタンスされたオブジェクトの取得用
    private Animator anim;
    private bool isShot;               //  ショット制御用

    [Header("弾のプレファブ")]
    public GameObject enemyShellPrefab;
    [Header("弾のスピード")]
    public float shotSpeed;
    [Header("ショット時のSE")]
    public AudioClip shotSound;
    [Header("ショットまでの待機時間の最小値と最大値")]
    public RangeInteger shotIntarvalRange;
    [Header("1回辺りの弾の生成数の最小値と最大値")]
    public RangeInteger shotCountRange;
    [Header("弾を撃つかどうか（Rはfalse Trueなら吐く）")]
    public bool isSplitter;

    private EnemyBase enemy;

    void Start() {
        if(!shooter) {
            shooter = transform.root.gameObject;
        }
        anim = shooter.GetComponent<Animator>();
        enemy = shooter.GetComponent<EnemyBase>();
        // 発射設定
        Reload();
    }
	
	void Update () {
        shotTimer += Time.deltaTime;

        if (shotIntarval < shotTimer){
            // 弾を生成しておらず、弾を生成できる敵なら弾を生成する
            if (!isShot && isSplitter) {
                isShot = true;
                StartCoroutine(Shot());
            }
        }
	}

    /// <summary>
    /// 弾の生成と発射処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Shot() {
        anim.SetBool("Idle", false);
        anim.SetBool("Walk", false);
        anim.SetTrigger("Fire");

        if (enemy.enemyType == EnemyType.MAGE) {
            // アニメと合わせるために少し待つ
            yield return new WaitForSeconds(0.4f);
        }
       
        for (int i = 0; i < shotCount; i++) {
            // 敵の位置から少し前に弾を生成する
            enemyShell = Instantiate(enemyShellPrefab, shooter.transform.position + new Vector3(0.15f * -shooter.transform.localScale.x, 0.4f, 0f), transform.rotation);
            float random = Random.Range(-2.5f, 2.5f);
            Rigidbody2D rb = enemyShell.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector3(-shotSpeed * shooter.transform.localScale.x, (rb.velocity.y + random), 0);
            Destroy(enemyShell, 3.0f);
        }
        Reload();        
    }

    /// <summary>
    /// 弾の生成準備
    /// </summary>
    private void Reload() {
        shotTimer = 0f;
        shotIntarval = Random.Range(shotIntarvalRange.MinValue, shotIntarvalRange.MaxValue);
        shotCount = Random.Range(shotCountRange.MinValue, shotCountRange.MaxValue);
        isShot = false;
    }
}
