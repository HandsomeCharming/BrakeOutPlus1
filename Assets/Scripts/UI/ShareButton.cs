using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareButton : MonoBehaviour {

    const string ScreenShotFilePath = "ScreenshotShare";

    public void Share()
    {
        ScreenCapture.CaptureScreenshot(ScreenShotFilePath);

        NativeShare share = new NativeShare();
        share.SetSubject("Test").SetText("FF");
        share.AddFile(ScreenShotFilePath);
        share.Share();
    }
}
