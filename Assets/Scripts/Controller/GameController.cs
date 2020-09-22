
﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : UiController, IPointerClickHandler
{
    public string ModeName;
    public string ButtonName;

    public RawImage Img;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //Debug.Log("Touched the UI");
            switch (ButtonName)
            {
                case "SceneChange":
                    LoadPlay(ModeName);
                    break;
                case "PauseButton":
                    LoadUI(false, true,false , false, false, false, false);
                    Time.timeScale = 0f;
                    break;
                case "BackButton":
                    LoadUI(true, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackButtonMM":
                    LoadUI(false, false, false, true, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackHome":
                    LoadUI(false, false, false, true, false, false, false);
                    Time.timeScale = 1f;
                    LoadPlay(ModeName);
                    break;
                case "Challenge":
                    print("Open Challenge Box");
                    break;
                case "Setting":
                    LoadUI(false, false, true, false, false, false, false);
                    Time.timeScale = 0f;
                    break;
                case "Gallery":
                    print("unknown");
                    break;
                case "PLayerInfo":
                    print("unknown");
                    break;
                case "UnPauseButton":
                    LoadUI(true, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "CaptureButton":
                    LoadUI(false, false, false, false, true, false, false);
                    Time.timeScale = 0f;
                    break;
                case "ExitOption":
                    LoadUI(true, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "ShareToInstagram":
                    StartCoroutine(TakeScreenshotAndShare());
                    break;
                case "SaveToGallery":
                    StartCoroutine(TakeScreenshotAndSave());
                    break;
                case "YesButton":
                    Application.Quit();
                    break;
                case "NoButton":
                    if (SceneManager.GetActiveScene().buildIndex == 0)
                    {
                        LoadUI(false, false, false, true, false, false, false);
                    }
                    else 
                    {
                        LoadUI(true, false, false, false, false, false, false);
                        Time.timeScale = 1f;
                    }
                    break;
                default:
                    print("Incorrect button Name");
                    break;
            }
        }
    }
    public void LoadUI(bool gpui, bool pause,bool sett, bool mainmenu,bool capture,bool exit,bool GOver)
    {
        GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = gpui;
        GameObject.FindGameObjectWithTag("CaptureOption").GetComponent<Canvas>().enabled = capture;
        GameObject.FindGameObjectWithTag("PauseOption").GetComponent<Canvas>().enabled = pause;
        GameObject.FindGameObjectWithTag("SettingOptionMM").GetComponent<Canvas>().enabled = sett;
        GameObject.FindGameObjectWithTag("ExitOption").GetComponent<Canvas>().enabled = exit;
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>().enabled = mainmenu;
        GameObject.FindGameObjectWithTag("GameOver").GetComponent<Canvas>().enabled = GOver;
    }

    public void LoadPlay(string Name)
    {

        SceneManager.LoadScene(Name, LoadSceneMode.Single);
    }
  
    //public IEnumerator CaptureScreen()
    //{
    //    // Wait till the last possible moment before screen rendering to hide the UI
    //    yield return null;
    //    GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

    //    // Wait for screen rendering to complete
    //    yield return new WaitForEndOfFrame();
    //    System.DateTime today = System.DateTime.Now;
    //    string day = today.ToString().Replace("/", "");
    //    string completeday = day.Replace(":", "");
    //    // Take screenshot
    //    //ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/RiverCapture" + completeday.Replace(" ", "") + ".png");
    //    string TempName = "RiverCapture" + completeday.Replace(" ", "") + ".png";
    //    StartCoroutine(TakeScreenshotAndSave());
    //    yield return new WaitForEndOfFrame();
    //    if (NativeGallery.IsMediaPickerBusy()) { yield return null; }

    //    // Show UI after we're done
    //    GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
    //}
    private IEnumerator TakeScreenshotAndSave()
    {   // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        // Wait till the last possible moment before screen rendering to hide the UI
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        string name = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name));

        //calculate the point
        
        StartCoroutine(CameraObjectManager.MyCamReceiver.capturedPointShot());
        PlayerController.MyPlayerControl.TakeDamage(10f);
        yield return null;
        // To avoid memory leaks
        Destroy(ss);
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animation>().Play();
        //ToastMessageShower.MyToast.showToastOnUiThread("Photo Saved in" + Application.productName + " Captures");
    }
    
    private IEnumerator TakeScreenshotAndShare()
    {
        print("CaptureScreen and share");
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);
        new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("Look At My Great Picture at River Horizon Game by GetDoIt")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget) )
            .Share();
            StartCoroutine(CameraObjectManager.MyCamReceiver.capturedPointShot());
        PlayerController.MyPlayerControl.TakeDamage(8f);

        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        //ToastMessageShower.MyToast.showToastOnUiThread("Photo Saved in" + Application.productName + " Captures");
        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
}