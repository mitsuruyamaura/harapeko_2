using System.Collections;
using UnityEngine;

public class FireWorks : MonoBehaviour {

    /// <summary>
    /// パーティクル再生処理に移行
    /// </summary>
    public void SetOffFireWorks(float wait){
        StartCoroutine(Fire(wait));
    }

    /// <summary>
    /// パーティクル再生の実処理
    /// </summary>
    /// <param name="wait"></param>
    /// <returns></returns>
    IEnumerator Fire(float wait) {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(wait); 
        particle.Play();
        // 花火と連動してエクセレントならボーナス宝石を出す判定処理を呼ぶ
        yield return new WaitForSeconds(0.2f);
        BonusJewels bonus = transform.parent.gameObject.GetComponent<BonusJewels>();
        bonus.CheckExcellentBonus();
        yield return new WaitForSeconds((wait + 1.5f));
        particle.Play();
        yield return new WaitForSeconds((wait + 2.0f));
        particle.Play();
    }
}
