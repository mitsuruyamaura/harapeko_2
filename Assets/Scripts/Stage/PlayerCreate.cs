using UnityEngine;

public class PlayerCreate : MonoBehaviour {

    [Header("キャラデータ")]
    public Character character;

    GameObject witch;

    private void Awake() {
        Debug.Log(StageManager.selectCharaNum);
        foreach(Character.CharaData chara in character.charaDatas) {
            if(chara.charaNum == StageManager.selectCharaNum) {
                GameObject witchPrefab = chara.charaObj;
                witch = Instantiate(witchPrefab, transform.position, Quaternion.identity);
                WitchMove witchMove = witch.GetComponent<WitchMove>();
                witchMove.SetUp(chara);
                break;
            }
        }
    }
}
