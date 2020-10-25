
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour, IPointerClickHandler
{
    public string ModeName;
    public string ButtonName;
    public CameraObjectManager COGM;
    public PlayerController PC;
    public UIManager UI;
    public GameManager GM;
    public RawImage Img;
   

    public void loadPC()
    {
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }
    public void LoadNeeded()
    {
        print("load");
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
    }
    //[System.Obsolete]
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left || Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            //Debug.Log("Touched the UI");
            
            switch (ButtonName)
            {
                case "SceneChange":
                    LoadPlay(ModeName);
                    break;
                case "PauseButton":
                    StartCoroutine(PauseButton());
                    break;
                case "BackButton":
                    LoadNeeded();
                    UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
                    Time.timeScale = 1f;
                    break;
                case "BackButtonMM":
                    StartCoroutine(BACKmm());
                    break;
                case "BackHome":
                    StartCoroutine(JustBackHome());
                    break;
                case "Challenge":
                    print("Open Challenge Box");
                    break;
                case "Setting":
                    StartCoroutine(SettingMain());
                    break;
                case "UpgradeMenu":
                    StartCoroutine(OpenShop());
                    break;
                case "Gallery":
                    LoadNeeded();
                    UI.LoadUI(false, false, false, false, false, false, false, false, true, false);
                    break;
                case "PreviewPhoto":
                    LoadNeeded();
                    UI.LoadUI(false, false, false, false, false, false, false, false, false, true);
                    break;
                case "PlayerInfo":
                   StartCoroutine(ShowEnergyValue());
                    break;
                case "UnPauseButton":
                    StartCoroutine(UnPauseButton());
                    break;
                case "CaptureButton":
                    StartCoroutine(Capturing());
                    //captureMoment();
                    break;
                case "ExitOption":
                    //doneCapture();
                    StartCoroutine(ExitOption());
                    break;
                case "ShareToInstagram":
                    StartCoroutine(Share());
                    break;
                case "SaveToGallery":
                    StartCoroutine(SaveOnly());
                    break;
                case "YesButton":
                    Application.Quit();
                    break;
                case "NoButton":
                    StartCoroutine(NoButton());
                    break;
                case "RestartButton":
                    LoadPlay(SceneManager.GetActiveScene().name);
                    break;
                case "SaveToPrefab":
                    //Saving Point to Prefab
                    StartCoroutine(SavingPoint());
                    if (ModeName.Equals("BackHomeOG"))
                    {
                        LoadPlay("MainMenu");
                    }
                    else
                    {
                        LoadPlay(SceneManager.GetActiveScene().name);
                    }
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
        animator.SetBool(Name, true);
        yield return new WaitForSecondsRealtime(3f);
    }
    public void LoadPlay(string Name)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAFterTransition(Name));

    }
    private IEnumerator TakeScreenshotAndSave()
    {
       
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;
        if (PC.Hidup && !GM.IsPaused)
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
            string name = string.Format("{0}_Capture_{1}.png", Application.productName, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, Application.productName + " Captures", name));
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
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
            UI.HPTOLOW();
            print("GAGAL");
            PC.IsAnimator.SetBool("IsCapture", false);
            GM.isCapturing = false;
            UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
            Time.timeScale = 1f;
            
        }
        else if (!PC.Hidup)
        {
            StartCoroutine(PC.Lose());

        }
        else
        {
            UI.HPTOLOW();
            print("GAGAL");
        }
        PC.jepretYes = true;
    }
    public void NullHandler()
    {
        LoadNeeded();
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // print(PC);
        // print(COGM);  
    }
    private IEnumerator TakeScreenshotAndShare()
    {
        
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;
        if (PC.Hidup && !GM.IsPaused)
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
            

            yield return null;
            GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        }
        else if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1))
        {
            UI.HPTOLOW();
            print("GAGAL");
            PC.IsAnimator.SetBool("IsCapture", false);
            GM.isCapturing = false;
            UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
            Time.timeScale = 1f;
        

        }
        else if (!PC.Hidup)
        {
            StartCoroutine(PC.Lose());
        }
        else
        {
            UI.HPTOLOW();
            print("GAGAL");
        }
        PC.jepretYes = true;
    }
    public IEnumerator ExitOption()
    {

        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;

        if (!PC.Hidup)
        {
            UI.LoadUI(false, false, false, false, false, false, true, false, false, false);
        }
        PC.IsAnimator.SetBool("IsCapture", false);
        GM.isCapturing = false;
        UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
        Time.timeScale = 1f;
        //GameObject.FindGameObjectWithTag("CoinParticle").GetComponent<ParticleSystem>().maxParticles = int.Parse(Mathf.Round(COGM.AllPoint).ToString());
        GameObject.Find("Canvas").GetComponent<Canvas>().worldCamera = Camera.main;
       
        if (PC.jepretYes )
        {
           
            StartCoroutine(COGM.capturedPointShot());
            PC.TakeDamage(8f);
            GameObject.FindGameObjectWithTag("CoinParticle").GetComponent<ParticleSystem>().Play();
            PC.jepretYes = false;
        }
       
        yield return null;
        yield break;
    }
    public IEnumerator Capturing()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
        PC = PlayerController.MyPlayerControl;
        PC.IsAnimator.SetBool("IsCapture", true);
        PC.IsAnimator.SetBool("IsMoving", false);
        PC.IsAnimator.SetBool("IsStop", false);
        GM.isCapturing = true;
        yield return null;
        yield break;
    }
    public IEnumerator PauseButton()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
        Animator PauseAnim = GameObject.FindGameObjectWithTag("PauseOption").GetComponent<Animator>();
        PauseAnim.SetBool("IsPaused", true);
        UI.LoadUI(false, true, false, false, false, false, false, false, false, false);
        yield return new WaitForEndOfFrame();
        yield break;
    }
    public IEnumerator OpenShop()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
        GM.isCapturing = true;
        UI.LoadUI(false, false, false, false, false, false, false, true, false, false);
        yield return null;
        yield break;
    }
    public IEnumerator SettingMain()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = GameObject.FindGameObjectWithTag("DistanceReceiver").GetComponent<CameraObjectManager>();
        UI.LoadUI(false, false, true, false, false, false, false, false, false, false);
        Time.timeScale = 0f;
        yield return null;
        yield break;
    }
    public IEnumerator SavingPoint()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;
        float str = PlayerPrefs.GetFloat("MyPoint");
        float str2 = PlayerPrefs.GetFloat("DistanceTraveled");
        int str3 = PlayerPrefs.GetInt("MyShot");
        if (!str.Equals(0f))
        {
            float calculatePointWithVar = UI.calculatePointWithVar;
            print("FP Saving " + calculatePointWithVar); 
            str = PlayerPrefs.GetFloat("MyPoint") + Mathf.Round(calculatePointWithVar);
            //print("Final Point:  "+str);
            PlayerPrefs.SetFloat("MyPoint", str);
        }
        else
        {
            PlayerPrefs.SetFloat("MyPoint", Mathf.Round(COGM.AllPoint));
        }
        if (!str2.Equals(0f))
        {
        
            str2 = PlayerPrefs.GetFloat("DistanceTraveled") + PC.totalDistance;
            PlayerPrefs.SetFloat("DistanceTraveled", str2);
        }
        else
        {
            PC.AllDistance += PC.totalDistance;
            PlayerPrefs.SetFloat("DistanceTraveled", Mathf.Round(PC.AllDistance));
        }
        if (!str3.Equals(0))
        {
            str3 = PlayerPrefs.GetInt("MyShot") + COGM.TempShotTaken;
            PlayerPrefs.SetInt("MyShot", str3);
        }
        else
        {
            PlayerPrefs.SetInt("MyShot", COGM.TempShotTaken);
        }
        PlayerPrefs.Save();
        yield break;
    }
    public IEnumerator NoButton()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            UI.LoadUI(false, false, false, true, false, false, false, false, false, false);
        }
        else
        {
            Canvas OptionMM = GameObject.FindGameObjectWithTag("BackOption").GetComponent<Canvas>();
            OptionMM.enabled = false;
            UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
            Time.timeScale = 1f;
        }
        yield return null;
        yield break;
    }
    public IEnumerator Share()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;
        if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1))
        {
            PC.IsAnimator.SetBool("IsCapture", false);
            GM.isCapturing = false;
            UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
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
        yield return null;
        yield break;
    }
    public IEnumerator SaveOnly()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        COGM = CameraObjectManager.MyCamReceiver;
        PC = PlayerController.MyPlayerControl;
        if (PC.myEnergy.MyCurrentValue <= (PC.myEnergy.MyMaxValue * 0.1))
        {

            PC.IsAnimator.SetBool("IsCapture", false);
            GM.isCapturing = false;
            UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
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
        yield return null;
        yield break;
    }
    public IEnumerator UnPauseButton()
    {
        LoadNeeded();
        Animator PauseAnim = GameObject.FindGameObjectWithTag("PauseOption").GetComponent<Animator>();
        PauseAnim.SetBool("IsPaused", false);
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        UI.LoadUI(true, false, false, false, false, false, false, false, false, false);
        Time.timeScale = 1f;
        yield return null;
        yield break;
    }
    public IEnumerator ShowEnergyValue()
    {
       
        UI = UIManager.MyUI;
        StartCoroutine(UI.ShowText());
        yield return null;
        yield break;
    }
    public IEnumerator BACKmm()
    {
        UI = UIManager.MyUI;
        UI.LoadUI(false, false, false, true, false, false, false, false, false, false);
        Time.timeScale = 1f;
        yield return null;
        yield break;
    }
    public IEnumerator JustBackHome()
    {
        UI = UIManager.MyUI;
        UI.LoadUI(false, false, false, true, false, false, false, false, false, false);
        Time.timeScale = 1f;
        LoadPlay(ModeName);
        Canvas OptionMM = GameObject.FindGameObjectWithTag("BackOption").GetComponent<Canvas>();
        OptionMM.enabled = false;
        yield return null;
        yield break;
    }

}
