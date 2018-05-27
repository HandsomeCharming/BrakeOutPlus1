using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShareButton : MonoBehaviour {

    public Camera screenshotCamera;
    public GameObject sharePanel;
    public Text scoreText;
    public Text highestScoreText;

    const string ScreenShotFilePath = "/ScreenshotShare.png";

    public void Share()
    {
        sharePanel.SetActive(true);
        scoreText.text = ((int)GameManager.current.gameScore).ToString();
        highestScoreText.text = ((int)GameManager.current.gameHighScore).ToString();
        screenshotCamera.gameObject.SetActive(true);
        screenshotCamera.forceIntoRenderTexture = true;
        screenshotCamera.Render();

        string path = Application.persistentDataPath + ScreenShotFilePath;
        print(path);
        SaveToFile(screenshotCamera.activeTexture, path);
        //ScreenCapture.CaptureScreenshot(ScreenShotFilePath);

        NativeShare share = new NativeShare();
        share.SetSubject("Test").SetText("FF");
        share.AddFile(path);
        share.Share();
        
        screenshotCamera.gameObject.SetActive(false);
        sharePanel.SetActive(false);
    }

    public void SaveToFile(RenderTexture renderTexture, string name)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        var bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(name, bytes);
        UnityEngine.Object.Destroy(tex);
        RenderTexture.active = currentActiveRT;
    }
}
