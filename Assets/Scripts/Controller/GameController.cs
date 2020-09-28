
using System.Collections;
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
    [SerializeField]
    private CameraObjectManager COGM;
    [SerializeField]
    private PlayerController PC;
    [SerializeField]
    private UIManager UI;
    [SerializeField]
    GameManager GM;
    public RawImage Img;

    [System.Obsolete]
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //Debug.Log("Touched the UI");
            UI = UIManager.MyUI;
            GM = GameManager.MyGM;
            PC = PlayerController.MyPlayerControl;
            COGM = CameraObjectManager.MyCamReceiver;
            switch (ButtonName)
            {
                case "SceneChange":
                    LoadPlay(ModeName);
                    break;
                case "PauseButton":
                    UI.LoadUI(false, true, false, false, false, false, false, false);
                    Time.timeScale = 0f;
                    break;
                case "BackButton":
                    UI.LoadUI(true, false, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackButtonMM":
                    UI.LoadUI(false, false, false, true, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackHome":
                    UI.LoadUI(false, false, false, true, false, false, false, false);
                    Time.timeScale = 1f;
                    LoadPlay(ModeName);
                    break;
                case "Challenge":
                    print("Open Challenge Box");
                    break;
                case "Setting":
                    UI.LoadUI(false, false, true, false, false, false, false, false);
                    Time.timeScale = 0f;
                    break;
                case "Gallery":
                    print("unknown");
                    break;
                case "PLayerInfo":
                    print("unknown");
                    break;
                case "UnPauseButton":
                    UI.LoadUI(true, false, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "CaptureButton":
                    PC = PlayerController.MyPlayerControl;
                    PC.IsAnimator.SetBool("IsCapture", true);
                    PC.IsAnimator.SetBool("IsMoving", false);
                    PC.IsAnimator.SetBool("IsStop", false);
                    GM.isCapturing = true;
                    //captureMoment();
                    break;
                case "ExitOption":
                    //doneCapture();
                    PC = PlayerController.MyPlayerControl;
                    PC.IsAnimator.SetBool("IsCapture", false);
                    GM.isCapturing = false;
                    UI.LoadUI(true, false, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    GameObject.FindGameObjectWithTag("CoinParticle").GetComponent<ParticleSystem>().maxParticles = int.Parse(Mathf.Round(COGM.AllPoint).ToString());
                    GameObject.FindGameObjectWithTag("CoinParticle").GetComponent<ParticleSystem>().Play();
                    break;
                case "ShareToInstagram":
                    COGM = CameraObjectManager.MyCamReceiver;
                    PC = PlayerController.MyPlayerControl;
                    if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1))
                    {
                        PC = PlayerController.MyPlayerControl;
                        PC.IsAnimator.SetBool("IsCapture", false);
                        GM.isCapturing = false;
                        UI.LoadUI(true, false, false, false, false, false, false, false);
                        Time.timeScale = 1f;
                    }
                    else if (!PC.Hidup)
                    {
                        PC.Lose();
                    }
                    else if (PC.myEnergy.MyCurrentValue >= (PC.myEnergy.MyMaxValue * 0.1))
                    {
                       
                        StartCoroutine(TakeScreenshotAndShare());
                       
                    }
                    break;
                case "SaveToGallery":
                    COGM = CameraObjectManager.MyCamReceiver;
                    PC = PlayerController.MyPlayerControl;
                    if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1))
                    {
                        PC = PlayerController.MyPlayerControl;
                        PC.IsAnimator.SetBool("IsCapture", false);
                        GM.isCapturing = false;
                        UI.LoadUI(true, false, false, false, false, false, false, false);
                        Time.timeScale = 1f;
                    }
                    else if (!PC.Hidup)
                    {
                        PC.Lose();
                    }
                    else if (PC.myEnergy.MyCurrentValue >= (PC.myEnergy.MyMaxValue * 0.1))
                    {
                        StartCoroutine(TakeScreenshotAndSave());
                    }
                    break;
                case "YesButton":
                    Application.Quit();
                    break;
                case "NoButton":
                    UI = UIManager.MyUI;
                    if (SceneManager.GetActiveScene().buildIndex == 0)
                    {
                        UI.LoadUI(false, false, false, true, false, false, false, false);
                    }
                    else
                    {
                        UI.LoadUI(true, false, false, false, false, false, false, false);
                        Time.timeScale = 1f;
                    }
                    break;
                case "RestartButton":
                    LoadPlay(SceneManager.GetActiveScene().name);
                    break;
                case "SaveToPrefab":
                    //Saving Point to Prefab
                    PC = PlayerController.MyPlayerControl;
                    //UI = UIManager.MyUI;
                    //StartCoroutine(UI.CalculatingPrefabPoint());

                    if (ModeName.Equals("BackHomeOG"))
                    {
                        LoadPlay("MainMenu");
                    }
                    else 
                    {
                        LoadPlay(SceneManager.GetActiveScene().name);
                    }
                    break;
                case "UpgradeMenu":
                    GM.isCapturing = true;
                    UI.LoadUI(false, false, false, false, false, false, false, true);
                    break;
                default:
                    print("Incorrect button Name");
                    break;
            }
        }
    }
    private Animator animator;
    private IEnumerator LoadSceneAFterTransition(string Name)
    {
        animator = GameObject.FindGameObjectWithTag("PanelTransisi").GetComponent<Animator>();
        //show animate out animation
        animator.SetBool("TutupYach", true);
        yield return new WaitForSecondsRealtime(3f);
    }
    public void LoadPlay(string Name)
    {
        StartCoroutine(LoadSceneAFterTransition(Name));

    }
    private IEnumerator TakeScreenshotAndSave()
    {
        if (PC.Hidup)
        {
            AudioController.Playsound("Jepret");
            Animator isShowing = GameObject.FindGameObjectWithTag("ShowImage").GetComponent<Animator>();
            isShowing.SetBool("ShowImage", false);
            Animator Jepret = GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animator>();
            Jepret.SetBool("Shutter", true);
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 1f;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
            // Wait till the last possible moment before screen rendering to hide the UI
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();
            COGM = CameraObjectManager.MyCamReceiver;
            // Save the screenshot to Gallery/Photos
            string name = string.Format("{0}_Capture{1}_{2}.png", Application.productName, "{0}", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name));

            //calculate the point
            if (PC.Hidup)
            {
                StartCoroutine(COGM.capturedPointShot());
                PC.TakeDamage(10f);
            }
            else
            {
                GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
                PC.Lose(); 
            }
            // To avoid memory leaks
            Destroy(ss);
            yield return null;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
            
            //kalau pause animasi ga jalan
            //Time.timeScale = 0f;
            //ToastMessageShower.MyToast.showToastOnUiThread("Photo Saved in" + Application.productName + " Captures");
        }
        else if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1)) 
        {
            PC = PlayerController.MyPlayerControl;
            PC.IsAnimator.SetBool("IsCapture", false);
            GM.isCapturing = false;
            UI.LoadUI(true, false, false, false, false, false, false, false);
            Time.timeScale = 1f;
        }
        else
        {
            PC.Lose();
        }
    }
    public void NullHandler()
    {
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
       // print(PC);
       // print(COGM);  
    }
    private IEnumerator TakeScreenshotAndShare()
    {
        if (PC.Hidup)
        {
            NullHandler();
            AudioController.Playsound("Jepret");
            Animator isShowing = GameObject.FindGameObjectWithTag("ShowImage").GetComponent<Animator>();
            isShowing.SetBool("ShowImage", false);
            Animator Jepret = GameObject.FindGameObjectWithTag("Shutter").GetComponent<Animator>();
            Jepret.SetBool("Shutter", true);
            yield return new WaitForSeconds(0.3f);
            Time.timeScale = 1f;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
            // Wait till the last possible moment before screen rendering to hide the UI
            yield return new WaitForEndOfFrame();

            Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            ss.Apply();
            COGM = CameraObjectManager.MyCamReceiver;
            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, ss.EncodeToPNG());

            // To avoid memory leaks
            Destroy(ss);
            new NativeShare().AddFile(filePath)
                .SetSubject("Subject goes here").SetText("Look At My Great Picture at River Horizon Game by GetDoIt")
                .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
                .Share();
            StartCoroutine(COGM.capturedPointShot());
            PC.TakeDamage(8f);

            yield return null;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        }
        else if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1))
        {
            PC = PlayerController.MyPlayerControl;
            PC.IsAnimator.SetBool("IsCapture", false);
            GM.isCapturing = false;
            UI.LoadUI(true, false, false, false, false, false, false, false);
            Time.timeScale = 1f;
        }
        else
        {
            PC.Lose();
        }
    }
    public void captureMoment()
    {
            PC.IsAnimator.SetBool("IsCapture", true);
            PC.IsAnimator.SetBool("IsMoving", false);
            PC.IsAnimator.SetBool("IsStop", false);
    }
    public void doneCapture()
    {
        NullHandler();
        PC.IsAnimator.SetBool("IsCapture", false);
    }

}
