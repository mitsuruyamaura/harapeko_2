using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharaData", menuName = "ScriptableObject/MakeCharaData")]
public class Character: ScriptableObject{

    public List<CharaData> charaDatas = new List<CharaData>();

    [System.Serializable]
    public class CharaData {
        [Header("キャラNo")]
        public int charaNum;
        [Header("表示キャラ")]
        public GameObject charaObj;
        [Header("移動速度")]
        public float moveSpeed;
        [Header("ジャンプ力")]
        public float jumpPower;
        [Header("チャージ攻撃時の消費ライフ量")]
        public int chargePoint;
        [Header("攻撃力")]
        public int attackPower;
        [Header("ライフ減少速度")]
        public float lifeLoss;
        [Header("スコアボーナス")]
        public float scoreBonus;
    }
}
