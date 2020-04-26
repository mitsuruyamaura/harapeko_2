using UnityEngine;
using UnityEngine.UI;

public class ToastText : MonoBehaviour {

    [Header("トーストテキスト")]
    public Text toast;

    /// <summary>
    /// 引数によってセーブ可否をポップアップ表示
    /// </summary>
    /// <param name="isSave"></param>
	public void DisplayToast(bool isSave) {
        if(isSave) {
            toast.text = "サーバーへのセーブに成功しました。\nハイスコア更新!!";
        } else {
            toast.text = "サーバー接続に失敗しました。\nハイスコアの更新に失敗…";
        }
    }
}
