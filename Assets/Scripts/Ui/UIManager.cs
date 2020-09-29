using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : UiController
{
    private GameManager GM;
    private PlayerController PL;
    private CameraObjectManager COGM;
    private static UIManager instance;
    public static UIManager MyUI
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    void Start()
    {
        NullHandler();
        COGM = CameraObjectManager.MyCamReceiver;
        PL = PlayerController.MyPlayerControl;
        GM = GameManager.MyGM;
        //DontDestroyOnLoad(transform.gameObject);
        //if (!UiControllerxist)
        //{
        //    UiControllerxist = true;
        //    DontDestroyOnLoad(transform.gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
    public void Update()
    {
        if (!GM.IsPaused && !GM.isCapturing && !GM.isDeath)
        {
            SwitchScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (GM.isDeath)
        {
            SwitchScene(4);
        }
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
                Activating(false, true);
                break;
            case 3:
                print("unknown");
                break;
            case 4:
                GameObject.FindGameObjectWithTag("GameOver").GetComponent<Canvas>().enabled = true;
                GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>().enabled = false;
                GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = false;
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }
    }
    public void Activating(bool Menu, bool Gameplay)
    {
        if (!GM.IsPaused && !GM.isCapturing && !GM.isDeath)
        {
            GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>().enabled = Menu;
            GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = Gameplay;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadUI(bool gpui, bool pause, bool sett, bool mainmenu, bool capture, bool exit, bool GOver,bool SnU)
    {
        GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = gpui;
        GameObject.FindGameObjectWithTag("CaptureOption").GetComponent<Canvas>().enabled = capture;
        GameObject.FindGameObjectWithTag("PauseOption").GetComponent<Canvas>().enabled = pause;
        GameObject.FindGameObjectWithTag("SettingOptionMM").GetComponent<Canvas>().enabled = sett;
        GameObject.FindGameObjectWithTag("ExitOption").GetComponent<Canvas>().enabled = exit;
        GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>().enabled = mainmenu;
        GameObject.FindGameObjectWithTag("GameOver").GetComponent<Canvas>().enabled = GOver;
        GameObject.FindGameObjectWithTag("ShopAndUpgrade").GetComponent<Canvas>().enabled = SnU;
    }
    public IEnumerator CalculatingPrefabPoint() 
    {
        CalculatePoint();
        yield return new WaitForSeconds(1);
        LoadUI(false, false, false, false, false, false, true, false);
        yield return null;
        /*yield return null;*/
    }
    public Text DistanceP;
    float tmpDistance;
    public Text CollectedPoint;
    float tmpCP;
    public Text SsTaken;
    float tmpSS;

    public void NullHandler()
    {
        if (CollectedPoint == null)
        {
            CollectedPoint = GameObject.FindGameObjectWithTag("CalculatedPoint").GetComponent<Text>();
        }
        if (DistanceP == null)
        {
            DistanceP = GameObject.FindGameObjectWithTag("DistancePoint").GetComponent<Text>();
        }
        if (SsTaken == null)
        {
            SsTaken = GameObject.FindGameObjectWithTag("ScreenShotTaken").GetComponent<Text>();
        }
    }
    public void CalculatePoint()
    {
        if ((CollectedPoint == null)||(DistanceP == null)||(SsTaken == null))
        {
            NullHandler();
        }

        COGM.tempShotTaken = COGM.InitShotTaken;
        COGM.AllPoint = COGM.PrevousPoint;
        PL.AllDistance = PL.totalDistance;
        StartCoroutine(PointTextHandleSS());
        StartCoroutine(PointTextHandleCP());
        StartCoroutine(PointTextHandleDP());
    }
    public IEnumerator PointTextHandleSS()
    {
        tmpSS = 0f;
        while (true)
        {
            if (tmpSS <= PL.AllShotTaken)
            {
                tmpSS++; //Increment the display score by 1
                SsTaken.text = Mathf.Round(Mathf.Lerp(tmpSS, PL.AllShotTaken, 0.1f * Time.deltaTime)).ToString();
            }
            yield return new WaitForSeconds(0.2f); // I used .2 secs but you can update it as fast as you want
        }

    }
    public IEnumerator PointTextHandleCP()
    {
        tmpCP = 0f;
        while (true)
        {
            if (tmpCP <= COGM.AllPoint)
            {
                tmpCP++; //Increment the display score by 1
                CollectedPoint.text = Mathf.Round(Mathf.Lerp(tmpCP, COGM.AllPoint, 0.1f * Time.deltaTime)).ToString();
            }
            yield return new WaitForSeconds(0.2f); // I used .2 secs but you can update it as fast as you want
        }

    }
    public IEnumerator PointTextHandleDP()
    {
        tmpDistance = 0f;
        while (true)
        {
            if (tmpDistance <= PL.AllDistance)
            {
                tmpDistance++; //Increment the display score by 1
                DistanceP.text = Mathf.Round(Mathf.Lerp(tmpDistance, PL.AllDistance, 0.1f * Time.deltaTime)).ToString();
            }
            yield return new WaitForSeconds(0.2f); // I used .2 secs but you can update it as fast as you want
        }

    }
}
