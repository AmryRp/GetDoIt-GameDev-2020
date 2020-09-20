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
        ObjectCatchs = new Dictionary<string, float>();
        KeyVal = new List<string>();
    }
    public IEnumerator AddingObjects()
    {
        int CountObjects = ObjectCatchs.Count;
        float tmpVal = 0;

        if (CountObjects != 0)
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
        if (CountObjects != 0)
        {
            foreach (KeyValuePair<string, float> Objects in ObjectCatchs)
            {
                tmpVal += Mathf.Round(Objects.Value);

            }
            capturePoint = 0f;
            capturePoint = tmpVal;
            AllPoint += capturePoint;
            CalculatePoint();
        }
        yield return null;
    }

    public void CalculateMeter()
    {
        if (CameraMeterBar == null)
        {
            CameraMeterBar = GameObject.FindGameObjectWithTag("CameraMeter").GetComponent<Image>();
        }
        bool isGolden = false;
        float calculateGolden;
        calculateGolden = CameraMeter / (maxCameraMeter - CameraMeter);
        if (calculateGolden == goldenRatio)
        {
            currentFill = 1f;
            isGolden = true;
        }
        else if (calculateGolden < goldenRatio)
        {
            currentFill = CameraMeter / maxCameraMeter;
            print("underrated");
        }
        else if (calculateGolden > goldenRatio)
        {
            currentFill = CameraMeter / maxCameraMeter;
            print("Overrated");
        }
        if (isGolden)
        {
            print("perfect");
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
            PointText = GameObject.FindGameObjectWithTag("CameraMeterText").GetComponent<Text>();
        }

    }
    public IEnumerator PointTextHandle()
    {

        if (PointText != null)
        {
            PointText.text = AllPoint.ToString();
        }
        yield return null;
    }
}
