using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotCapturer : MonoBehaviour
{
    public bool screenShotEnabled = false;
    public int screenShotIndex = 0;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && screenShotEnabled){
            string name = "ScreenShot_" + screenShotIndex + ".png";
            ScreenCapture.CaptureScreenshot(name);
            screenShotIndex++;
        }
    }
}
