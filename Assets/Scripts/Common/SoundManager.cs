using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

    public enum BGM_TYPE {
        TITLE,
        STAGE_1,
        STAGE_2,
        STAGE_3,
        STAGE_4,
        STAGE_5,
        BONUS_STAGE,
        CREDIT,
    }

    public enum SE_TYPE {
        JUMP,
        NORMAL_ATTACK,
        CHARGE_ATTACK,
        CHARGE,
        POWER_UP,
        STAGE_CLAER,
        DAMEGE,
        RECOVER,
        GAME_OVER,
        HIT,
        ENEMY_DOWN,
        BTN_OK,
    }

    [Header("BGM用の音源")]
    public AudioClip[] bgm;

    [Header("SE用の音源")]
    public AudioClip[] se;

    [Header("BGM用の再生プレイヤー")]
    public AudioSource bgmSource;   // 鳴らす音源はAudioClipを差し替えて鳴らすので、インスペクターのAudioClipはNoneでよい
                                    // フェイドインさせるので、Volumeは0にしておく

    private AudioSource[] seSources = new AudioSource[16];

    public float fadeTime = 2.0f;

    private int index = -1;  // 現在鳴っているBGMの番号(配列の番号と連動)


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
        // SE用AudioSourceを生成しSoundManagerにアタッチ
        for (int i = 0; i < seSources.Length; i++) {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    ///// <summary>
    ///// ゲームオーバー時のBGM処理
    ///// </summary>
    //public void StopBgm() {
    //    stageBGM.Stop();
    //    gameOverSE.PlayOneShot(gameOverSE.clip);
    //}

    /// <summary>
    /// ステージクリア時のBGM処理
    /// </summary>
    //public void GameClear(){
    //    stageBGM.Stop();
    //    if (!clearBGM.isPlaying){
    //        clearBGM.PlayOneShot(clearBGM.clip);
    //    }
    //}

    /// <summary>
    /// ライフ回復SE再生
    /// </summary>
    //public void RecoverLifeSE() {
    //    lifeRecoverSE.PlayOneShot(lifeRecoverSE.clip);
    //}

    /// <summary>
    /// ライフ減少SE再生
    /// </summary>
    //public void DamageLifeSE() {
    //    lifeDownSE.PlayOneShot(lifeDownSE.clip);
    //}

    /// <summary>
    /// 敵の被ダメージ時のSE再生
    /// </summary>
    //public void EnemyDamageSePlay(Vector2 pos) {
    //    AudioSource.PlayClipAtPoint(enemyDamagedSe, pos);
    //    Debug.Log("damage");
    //}


    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="type"></param>
    public void PlayBgm(BGM_TYPE type) {
        // すでにBGMが再生されている場合、異なる音源のみ再生するようにチェックする
        if (index == (int)type) {
            // 再生されている音源と同じ場合には何もしないで終了
            return;
        }
        // BGM_TYPEをint型にキャストして配列の番号に対応させる
        index = (int)type;

        // BGM用再生プレイヤーに音源をセットする
        bgmSource.clip = bgm[index];

        // BGMを再生
        bgmSource.Play();

        // Volumeを0にしておいて徐々に大きくして1(Max)にする
        // Maxになるまでの時間はfadeTimeで指定する
        bgmSource.DOFade(1, fadeTime);
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    /// <returns></returns>
    public IEnumerator StopBGM() {
        // BGMのVolumeを徐々に下げて0(Min)にする
        bgmSource.DOFade(0, fadeTime);

        // 下げている時間だけ待機する
        yield return new WaitForSeconds(fadeTime);

        // BGMを止める
        bgmSource.Stop();

        // セットされている音源を空にする
        bgmSource.clip = null;

        // 再生が終了したので配列用の番号を初期化する
        index = -1;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seNo"></param>
    public void PlaySE(SE_TYPE type) {
        int seIndex = (int)type;

        // 再生中で無いAudioSouceで鳴らす
        foreach (AudioSource source in seSources) {
            // AudioSource.isPlayingで再生中かどうかboolで取れる
            if (!source.isPlaying) {
                // SE用の音源をセットする
                source.clip = se[seIndex];
                // SEを再生
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE() {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in seSources) {
            // SEを停止
            source.Stop();
            // セットされている音源を空にする
            source.clip = null;
        }
    }
}
