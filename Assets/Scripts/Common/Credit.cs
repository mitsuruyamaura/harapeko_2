using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CreditData",menuName = "ScriptableObject/CreateCreditData"),]
public class Credit : ScriptableObject {

    public List<CreditData> creditDatas = new List<CreditData>();

    [System.Serializable]
    public class CreditData {
        [SerializeField, Header("No")]
        public int no;
        [SerializeField, Header("クレジット名")]
        public string companyName;
        [SerializeField, Header("素材種類名")]
        public string assetName;
    }
}
