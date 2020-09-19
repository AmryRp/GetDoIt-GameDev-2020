using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : UiController, IPointerClickHandler
{
    public string ModeName;
    public string ButtonName;
    
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            Debug.Log("Touched the UI");
            switch (ButtonName)
            {
                case "SceneChange":
                    LoadPlay(ModeName);
                    break;
                case "PauseButton":
                    PauseButton();
                    break;
                case "BackButton":
                    print("Time Mode");
                    break;
                case "Challenge":
                    print("unknown");
                    break;
                case "Setting":
                    print("unknown");
                    break;
                case "Gallery":
                    print("unknown");
                    break;
                case "PLayerInfo":
                    print("unknown");
                    break;
                case "UnPauseButton":
                    UnPauseButton();
                    break;
                case "CaptureButton":
                    OnClickScreenCaptureButton();
                    break;
                default:
                    print("Incorrect button Name");
                    break;
            }
        }
    }
    public void PauseButton() 
    {
        print("PAUSED");
        GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = false;
        GameObject.FindGameObjectWithTag("PauseOption").GetComponent<Canvas>().enabled = true;
        
        Time.timeScale = 0f;
    }
    public void UnPauseButton()
    {
        print("UNPAUSED");
        GameObject.FindGameObjectWithTag("PauseOption").GetComponent<Canvas>().enabled = false;
        GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = true;
        Time.timeScale = 1f;
    }
    public void LoadPlay(string Name)
    {

        SceneManager.LoadScene(Name, LoadSceneMode.Single);
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
        string completeday = day.Replace(":", "");
        // Take screenshot
        //ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/RiverCapture" + completeday.Replace(" ", "") + ".png");
        string TempName = "RiverCapture" + completeday.Replace(" ", "") + ".png";


        StartCoroutine(TakeScreenshotAndSave());

        yield return new WaitForEndOfFrame();
        PickImage(1024);
        if (NativeGallery.IsMediaPickerBusy()) { yield return null; }

        // Show UI after we're done
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
    }
    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        //NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));
        string name = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name));
       // Debug.Log("Permission result: " + permission);
       // Debug.Log("Permission result: " + permission);

        // To avoid memory leaks
        Destroy(ss);
    }

    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                quad.transform.forward = Camera.main.transform.forward;
                quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                Material material = quad.GetComponent<Renderer>().material;
                if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                material.mainTexture = texture;

                Destroy(quad, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
                Destroy(texture, 5f);
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
    }


}

