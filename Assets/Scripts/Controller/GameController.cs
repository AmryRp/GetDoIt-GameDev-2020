
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
    private CameraObjectManager COGM;
    private PlayerController PC;
    public RawImage Img;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //Debug.Log("Touched the UI");
            UIManager UI = UIManager.MyUI;
            GameManager GM = GameManager.MyGM;
            switch (ButtonName)
            {
                case "SceneChange":
                    LoadPlay(ModeName);
                    break;
                case "PauseButton":
                    UI.LoadUI(false, true,false , false, false, false, false);
                    Time.timeScale = 0f;
                    break;
                case "BackButton":
                    UI.LoadUI(true, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackButtonMM":
                    UI.LoadUI(false, false, false, true, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackHome":
                    UI.LoadUI(false, false, false, true, false, false, false);
                    Time.timeScale = 1f;
                    LoadPlay(ModeName);
                    break;
                case "Challenge":
                    print("Open Challenge Box");
                    break;
                case "Setting":
                    UI.LoadUI(false, false, true, false, false, false, false);
                    Time.timeScale = 0f;
                    break;
                case "Gallery":
                    print("unknown");
                    break;
                case "PLayerInfo":
                    print("unknown");
                    break;
                case "UnPauseButton":
                    UI.LoadUI(true, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "CaptureButton":
                    GM.isCapturing = true;
                    UI.LoadUI(false, false, false, false, true, false, false);
                    
                    break;
                case "ExitOption":
                    doneCapture();
                    GM.isCapturing = false;
                    UI.LoadUI(true, false, false, false, false, false, false);
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
                        UI.LoadUI(false, false, false, true, false, false, false);
                    }
                    else 
                    {
                        UI.LoadUI(true, false, false, false, false, false, false);
                        Time.timeScale = 1f;
                    }
                    break;
                case "RestartButton":
                    LoadPlay(SceneManager.GetActiveScene().name);
                    break;
                default:
                    print("Incorrect button Name");
                    break;
            }
        }
    }
    private void Awake()
    {
        PC = PlayerController.MyPlayerControl;
        COGM = CameraObjectManager.MyCamReceiver;
    }

    public void LoadPlay(string Name)
    {

        SceneManager.LoadScene(Name, LoadSceneMode.Single);
    }
  
    private IEnumerator TakeScreenshotAndSave()
    {

        yield return new WaitForSeconds(0.5f);
        captureMoment();
        AudioController.Playsound("Jepret");
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animator>().enabled = false;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        COGM.InitShotTaken += 1f;
        // Save the screenshot to Gallery/Photos
        string name = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name));

        //calculate the point
        
        StartCoroutine(COGM.capturedPointShot());
        PC.TakeDamage(10f);
        yield return new WaitForSeconds(1.2f);
        // To avoid memory leaks
        Destroy(ss);
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(2f);
        doneCapture();
        //kalau pause animasi ga jalan
        //Time.timeScale = 0f;
        //ToastMessageShower.MyToast.showToastOnUiThread("Photo Saved in" + Application.productName + " Captures");
    }

    private IEnumerator TakeScreenshotAndShare()
    {

        yield return new WaitForSeconds(1f);
        captureMoment();
        AudioController.Playsound("Jepret");
        yield return new WaitForSeconds(0.3f);
        GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animator>().enabled = false;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();
        COGM.InitShotTaken += 1f;
        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);
        new NativeShare().AddFile(filePath)
            .SetSubject("Subject goes here").SetText("Look At My Great Picture at River Horizon Game by GetDoIt")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget) )
            .Share();
            StartCoroutine(COGM.capturedPointShot());
        PC.TakeDamage(8f);

        yield return new WaitForSeconds(2f);
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        yield return new WaitForSeconds(2f);
        doneCapture();
        //kalau pause animasi ga jalan
        //Time.timeScale = 0f;
        //ToastMessageShower.MyToast.showToastOnUiThread("Photo Saved in" + Application.productName + " Captures");
        // Share on WhatsApp only, if installed (Android only)
        //if( NativeShare.TargetExists( "com.whatsapp" ) )
        //	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
    }
    public void captureMoment()
    {
        PC.IsAnimator.SetBool("IsCapture", true);
        PC.IsAnimator.SetBool("IsMoving", false);
        PC.IsAnimator.SetBool("IsStop", false);
    }
    public void doneCapture()
    {
        PC.IsAnimator.SetBool("IsCapture", false);
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
}
