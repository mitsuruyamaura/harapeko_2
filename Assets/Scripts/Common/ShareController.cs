using UnityEngine;
using System.IO;
using System.Collections;
using SocialConnector;
using UnityEngine.UI;

public class ShareController : MonoBehaviour {
    public void Share() {
        StartCoroutine(ShareScreenShot());
    }

    IEnumerator ShareScreenShot() {
        //ファイル名が重複しないように実行時間を付与。
        string fileName = System.DateTime.Now.ToString("Screen yyyy-MM-dd HH.mm.ss") + ".png";

        //スクリーンショット画像の保存先を設定。
        string imagePath = Application.persistentDataPath + "/" + fileName;

        //スクリーンショットを撮影
        ScreenCapture.CaptureScreenshot(fileName);

        yield return new WaitForEndOfFrame();

        // アプリやシーンごとにShareするメッセージを設定
        string text = "ツイート内容\n#hashtag ";
        string URL = "url";
        yield return new WaitForSeconds(1);

        //Shareする
        SocialConnector.SocialConnector.Share(text, URL, imagePath);
    }
}
