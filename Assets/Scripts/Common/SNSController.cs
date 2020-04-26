using System.Collections;
using UnityEngine;
using System.IO;
using SocialConnector;
using UnityEngine.UI;

public class SNSController : MonoBehaviour {

	public void Share() {
        StartCoroutine(ShareScreenShot());
    }

    public IEnumerator ShareScreenShot() {
        
        // ファイル名が重複しないよう実行時間を付ける
        string fileName = System.DateTime.Now.ToString("ScreenShot 777-MM-dd HH.mm.ss") + ".png";

        // スクショ画像の保存先を設定
        string imagePath = Application.persistentDataPath + "/" + fileName;

        // スクショを撮影
        ScreenCapture.CaptureScreenshot(fileName);

        yield return new WaitForEndOfFrame();

        // アプリやシーンごとにShareするメッセージを設定
        string text = "ツイート内容\n#hashtag";
        string URL = "url";
        yield return new WaitForSeconds(1.0f);

        // Shareする
        SocialConnector.SocialConnector.Share(text,URL,imagePath);
    }
}
