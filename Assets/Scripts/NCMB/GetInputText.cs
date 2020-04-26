using UnityEngine;
using UnityEngine.UI;

public class GetInputText : MonoBehaviour {

    [Header("ハイスコア表示用")]
    public Text inputText;

    /// <summary>
    /// InputFeildで入力された文字を取得して戻す
    /// </summary>
    /// <returns></returns>
    public string GetText() {
        string name = inputText.text;
        return name;
    }
}
