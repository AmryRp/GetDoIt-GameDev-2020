using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : UiController
{
    public GameManager GM;
    public PlayerController PL;
    public CameraObjectManager COGM;
    private static UIManager instance;
    public Canvas Gpui, Pause, Sett, Mainmenu, Capture, Exit, GmOver, SavenU, GalView, PhotoView;
    public Text EnergyStats;
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
        COGM = CameraObjectManager.MyCamReceiver;
        PL = PlayerController.MyPlayerControl;
        GM = GameManager.MyGM;

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
                StartCoroutine(PL.Lose());
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
            Mainmenu.GetComponent<Canvas>().enabled = Menu;
            Gpui.GetComponent<Canvas>().enabled = Gameplay;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadUI(bool gpui, bool pause, bool sett, bool mainmenu, bool capture, bool exit, bool GOver, bool SnU, bool GV, bool PV)
    {
        Gpui.GetComponent<Canvas>().enabled = gpui;
        Pause.GetComponent<Canvas>().enabled = pause;
        Exit.GetComponent<Canvas>().enabled = exit;
        Mainmenu.GetComponent<Canvas>().enabled = mainmenu;
        Capture.GetComponent<Canvas>().enabled = capture;
        GmOver.GetComponent<Canvas>().enabled = GOver;
        SavenU.GetComponent<Canvas>().enabled = SnU;
        Sett.GetComponent<Canvas>().enabled = sett;
        GalView.GetComponent<Canvas>().enabled = GV;
        PhotoView.GetComponent<Canvas>().enabled = PV;
    }
    public IEnumerator CalculatingPrefabPoint()
    {
        CalculatePoint();
        yield return new WaitForSeconds(0.1f);
        yield break;
        /*yield return null;*/
    }
    public Text DistanceP;
    float tmpDistance;
    float AllDistance;
    public Text CollectedPoint;
    float tmpCP;
    float AllPointCol;
    public Text SsTaken;
    int tmpSS;
    int AllTakenSS;
    public Text FinalPoint;
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
        if (FinalPoint == null)
        {
            FinalPoint = GameObject.FindGameObjectWithTag("FinalPoint").GetComponent<Text>();
        }
    }
    public void CalculatePoint()
    {
        if ((CollectedPoint == null) || (DistanceP == null) || (SsTaken == null) || (FinalPoint == null))
        {
            NullHandler();
        }

        // COGM.TempShotTaken += COGM.InitShotTaken;
        AllTakenSS = COGM.TempShotTaken;
        // COGM.AllPoint += COGM.PrevousPoint;
        AllPointCol = COGM.AllPoint;
        //PL.AllDistance += PL.totalDistance;
        AllDistance = PL.totalDistance;
        StartCoroutine(PointTextHandleSS());
        StartCoroutine(PointTextHandleCP());
        StartCoroutine(PointTextHandleDP());
        StartCoroutine(PointTextHandleFinal());

    }
    public IEnumerator PointTextHandleSS()
    {
        print("SS " + AllTakenSS);
        tmpSS = 0;
        while (tmpSS < AllTakenSS)
        {
            tmpSS++; //Increment the display score by 1
            SsTaken.text = Mathf.Lerp(tmpSS, AllTakenSS, 1 * Time.unscaledDeltaTime).ToString();
            yield return null;
        }
        SsTaken.text = Mathf.Round(AllTakenSS).ToString();
        yield break;
    }
    public IEnumerator PointTextHandleCP()
    {
        print("CP " + AllPointCol);
        tmpCP = 0f;
        while (tmpCP < AllPointCol)
        {
            tmpCP++; //Increment the display score by 1
            CollectedPoint.text = Mathf.Round(Mathf.Lerp(tmpCP, AllPointCol, 0.1f * Time.unscaledDeltaTime)).ToString();
            yield return null;
        }
        CollectedPoint.text = Mathf.Round(AllPointCol).ToString();
        yield break;
    }
    public IEnumerator PointTextHandleDP()
    {
        print("DP " + AllDistance);
        tmpDistance = 0f;
        while (tmpDistance < AllDistance)
        {
            tmpDistance++; //Increment the display score by 1
            DistanceP.text = Mathf.Round(Mathf.Lerp(tmpDistance, AllDistance, 0.1f * Time.unscaledDeltaTime)).ToString();
            yield return null;
        }
        DistanceP.text = Mathf.Round(AllDistance).ToString();
        yield break;
    }
    public IEnumerator PointTextHandleFinal()
    {

        float calculatePointWithVar = (Mathf.Round((COGM.AllPoint) * (COGM.TempShotTaken + 1)) / 5) + (Mathf.Round(PL.AllDistance) / COGM.TempShotTaken);
        print("FP " + calculatePointWithVar);
        tmpDistance = 0f;
        while (tmpDistance < calculatePointWithVar)
        {
            tmpDistance++; //Increment the display score by 1
            FinalPoint.text = Mathf.Round(Mathf.Lerp(tmpDistance, calculatePointWithVar, 0.1f * Time.unscaledDeltaTime)).ToString();
            yield return null;
        }
        FinalPoint.text = Mathf.Round(calculatePointWithVar).ToString();
        yield break;
    }
    public IEnumerator ShowText()
    {

        for (float i = 0; i <= 1f; i += 0.05f)
        {

            EnergyStats.color = new Color(0f, 0f, 0f, Mathf.SmoothStep(0, 1, i));
            yield return new WaitForSeconds(0.01f);
        }
        EnergyStats.color = new Color(0f, 0f, 0f, 1);
        yield return new WaitForSeconds(5f);
        StartCoroutine(HideText());
    }
    public IEnumerator HideText()
    {
        for (float i = 1; i >= 0; i -= 0.03f)
        {
            EnergyStats.color = new Color(0f, 0f, 0f, Mathf.SmoothStep(0, 1, i));
            yield return new WaitForSeconds(0.01f);
        }
        EnergyStats.color = new Color(0f, 0f, 0f, 0);
    }
    public void HPTOLOW()
    {
        print("Your Energy Is Too Low");
        SSTools.ShowMessage("Your Energy Is Too Low", SSTools.Position.bottom, SSTools.Time.twoSecond);

    }
}
