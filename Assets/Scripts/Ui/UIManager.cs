using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : UiController
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickScreenCaptureButton()
    {
        StartCoroutine(CaptureScreen());
    }
    public IEnumerator CaptureScreen()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();
        System.DateTime today = System.DateTime.Now;
        string day = today.ToString().Replace("/", "");
        string completeday = day.Replace(":","");
        // Take screenshot
        ScreenCapture.CaptureScreenshot(Application.persistentDataPath +"/RiverCapture" + completeday.Replace(" ","") + ".png");

        // Show UI after we're done
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
    }
}
