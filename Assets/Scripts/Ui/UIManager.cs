using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : UiController
{
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (!UiControllerxist)
        {
            UiControllerxist = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        SwitchScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SwitchScene(int SceneName) 
    {
        switch (SceneName)
        {
            case 0:
                Activating(true, false);
                break;
            case 1:
                Activating(false, true);
                break;
            case 2:
                print("Time Mode");
                break;
            case 3:
                print("unknown");
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }
    }
    public void Activating(bool Menu, bool Gameplay)
    {

        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>().enabled = Menu;
        GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = Gameplay;
    }
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
