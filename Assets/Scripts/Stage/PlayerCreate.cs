using UnityEngine;

/// <summary>
/// ステージ開始時にタイトルで選択したキャラを生成
/// </summary>
public class PlayerCreate : MonoBehaviour {

    [Header("キャラデータ")]
    public Character character;

    GameObject witch;

    private void Awake() {
        // タイトルで選択したキャラ生成
        CreateChara();
    }

    /// <summary>
    /// ステージ開始時にタイトルで選択したキャラを生成
    /// </summary>
    private void CreateChara() {
        Debug.Log(GameData.instance.selectCharaNum);
        foreach (Character.CharaData chara in character.charaDatas) {
            if (chara.charaNum == GameData.instance.selectCharaNum) {
                GameObject witchPrefab = chara.charaObj;
                witch = Instantiate(witchPrefab, transform.position, Quaternion.identity);
                WitchMove witchMove = witch.GetComponent<WitchMove>();
                witchMove.SetUp(chara);
                break;
            }
        }
    }
}
