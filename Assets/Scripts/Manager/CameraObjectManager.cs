using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CameraObjectManager : MonoBehaviour
{
    public ArrayList ObjectHasCatch;
    [SerializeField]
    public Dictionary<string, float> ObjectCatchs;
    public List<string> KeyVal;
    public float CameraMeter;
    public Image CameraMeterBar;
    private float currentFill;
    public float maxCameraMeter = 40f;
    public static float goldenRatio = 1.618f;
    public float capturePoint;
    public float AllPoint;
    public Text PointText;
    public float PrevousPoint;
    public int MaxObjects = 45;
    public float InitShotTaken;
    public float tempShotTaken;
    private static CameraObjectManager instance;
    public static CameraObjectManager MyCamReceiver

    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraObjectManager>();
            }
            return instance;
        }
    }
    private void Start()
    {
        AllPoint = 0f;
        //PointText.text = "0";
        InitShotTaken = 0f;
        ObjectCatchs = new Dictionary<string, float>();
        KeyVal = new List<string>();
    }

    public IEnumerator AddingObjects()
    {
        int CountObjects = ObjectCatchs.Count;
        float tmpVal = 0;

        if (CountObjects != 0 && CountObjects <= MaxObjects)
        {
            foreach (KeyValuePair<string, float> Objects in ObjectCatchs)
            {
                tmpVal += Objects.Value;

            }
            CameraMeter = 0f;
            CameraMeter = tmpVal / CountObjects;
        }

        yield return null;
    }
    public IEnumerator capturedPointShot()
    {
        int CountObjects = ObjectCatchs.Count;
        float tmpVal = 0;
        float IPower = 0;
        if (CountObjects != 0)
        {
            foreach (KeyValuePair<string, float> Objects in ObjectCatchs)
            {
                tmpVal += Mathf.Round(Objects.Value);
                char separator = "_"[0];
                string pow = Objects.Key.Split(separator)[2];
                IPower += float.Parse(pow);

            }
            capturePoint = 0f;
            IPower = IPower * calculateGolden;
            capturePoint = (tmpVal + IPower) / CountObjects;
            PrevousPoint = AllPoint;
            AllPoint += capturePoint;
            InitShotTaken += 1f;
            tempShotTaken = InitShotTaken;
        }
        CalculatePoint();
        yield return null;
    }
    public float calculateGolden = 1f;
    public void CalculateMeter()
    {
        if (CameraMeterBar == null)
        {
            CameraMeterBar = GameObject.FindGameObjectWithTag("CameraMeter").GetComponent<Image>();
        }
        bool isGolden = false;

        calculateGolden = CameraMeter / (maxCameraMeter - CameraMeter);
        if (calculateGolden == goldenRatio)
        {
            currentFill = 1f;
            isGolden = true;
            CameraMeterBar.color = Color.cyan;
        }
        else if (calculateGolden < goldenRatio)
        {
            currentFill = ((CameraMeter * calculateGolden) * 0.5f) / maxCameraMeter;
            CameraMeterBar.color = Color.green;
        }
        else if (calculateGolden > goldenRatio)
        {
            currentFill = ((CameraMeter * calculateGolden) * 1f) / maxCameraMeter;
            CameraMeterBar.color = Color.yellow;
        }
        else
        {
            CameraMeterBar.color = Color.white;
        }
        if (isGolden)
        {
            //tampilkan teks
            //print("perfect");
        }
        StartCoroutine(HandleBar());
    }
    public IEnumerator HandleBar()
    {

        if (currentFill != CameraMeterBar.fillAmount)
        {
            CameraMeterBar.fillAmount = Mathf.MoveTowards(CameraMeterBar.fillAmount, currentFill, Time.time * 0.07f);
        }
        yield return null;
    }
    public void CalculatePoint()
    {
        if (PointText == null)
        {
            PointText = GameObject.FindGameObjectWithTag("PointText").GetComponent<Text>();
        }
        StartCoroutine(PointTextHandle());
    }
    public IEnumerator PointTextHandle()
    {
        while (true)
        {
            if (PrevousPoint < AllPoint)
            {
                PrevousPoint++; //Increment the display score by 1
                PointText.text = Mathf.Round(Mathf.Lerp(PrevousPoint, AllPoint, 0.1f * Time.deltaTime)).ToString();
            }
            yield return new WaitForSeconds(0.2f); // I used .2 secs but you can update it as fast as you want
        }
        
    }
}
