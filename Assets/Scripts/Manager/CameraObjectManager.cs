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
    private float allPoint;
    public Text PointText;
    public float PrevousPoint;
    public int MaxObjects = 45;
    public int InitShotTaken;
    private int tempShotTaken;
    public bool isGolden = false;
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

    public float AllPoint { get => allPoint; set => allPoint = value; }
    public int TempShotTaken { get => tempShotTaken; set => tempShotTaken = value; }

    private void Start()
    {
        AllPoint = 0f;
        //PointText.text = "0";
        InitShotTaken = 0;
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
            CameraMeter = tmpVal / (CountObjects * goldenRatio);
        }

        yield return null;
    }
    public IEnumerator capturedPointShot()
    {
        int CountObjects = ObjectCatchs.Count;
        float tmpVal = 0;
        float IPower = 0;
        float countanimals = 0;
        float toAcomplish = 0;
        if (CountObjects != 0)
        {
            foreach (KeyValuePair<string, float> Objects in ObjectCatchs)
            {
                tmpVal += Mathf.Round(Objects.Value);
                char separator = "_"[0];
                string pow = Objects.Key.Split(separator)[2];
                string name = Objects.Key.Split(separator)[0];
                if (name.Equals(ObjectivesManager.MyInstance.animalName))
                {
                    CountObjects++;
                    print(ObjectivesManager.MyInstance.animalName+" "+CountObjects);
                }
                IPower += float.Parse(pow);

            }
            capturePoint = 0f;
            IPower = IPower * calculateGolden;
            capturePoint = (tmpVal + IPower) / (CountObjects * goldenRatio * (InitShotTaken+1));
            PrevousPoint = AllPoint;
            if (isGolden)
            {
                float point = capturePoint * 1.5f;
                capturePoint = point;
            }
            AllPoint += capturePoint;
            
            InitShotTaken++;
            tempShotTaken = InitShotTaken;
            toAcomplish = InitShotTaken;
        }
        ObjectivesManager.MyInstance.AcomplishObjective(toAcomplish, "photos", 1);
        ObjectivesManager.MyInstance.AcomplishObjective(countanimals, ObjectivesManager.MyInstance.animalName, 2);
        CalculatePoint();
        yield break;
    }
    public float calculateGolden = 1f;
    public void CalculateMeter()
    {
        if (CameraMeterBar == null)
        {
            CameraMeterBar = GameObject.FindGameObjectWithTag("CameraMeter").GetComponent<Image>();
        }

        if (CameraMeter < maxCameraMeter)
        {
            calculateGolden = (maxCameraMeter + CameraMeter) / maxCameraMeter;
        }
        else
        {
            calculateGolden = (maxCameraMeter - CameraMeter) / CameraMeter;
        }
        if (calculateGolden >= 1.35f && calculateGolden <= goldenRatio)
        {
            float rand = UnityEngine.Random.Range(0.6f, 0.9f);
            currentFill = rand;
            isGolden = true;
        }

        if (calculateGolden == goldenRatio || calculateGolden >= 1.5 && calculateGolden <= goldenRatio)
        {
            currentFill = 1f;
            isGolden = true;
            CameraMeterBar.color = Color.cyan;
        }
        else if (calculateGolden < goldenRatio)
        {
            currentFill = ((CameraMeter * calculateGolden) * 1f) / maxCameraMeter;
            CameraMeterBar.color = Color.yellow;
        }
        else if (calculateGolden > goldenRatio)
        {
            currentFill = ((CameraMeter * calculateGolden) * 1.25f) / maxCameraMeter;
            CameraMeterBar.color = Color.green;
        }
        else
        {
            CameraMeterBar.color = Color.white;
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
        while (PrevousPoint < AllPoint)
        {
            PrevousPoint++; //Increment the display score by 1
            PointText.text = Mathf.Round(Mathf.Lerp(PrevousPoint, AllPoint, 0.1f * Time.deltaTime)).ToString();
        }
        yield return new WaitForSeconds(0.01f);
        if(PrevousPoint >= AllPoint)
        {
            PointText.text = Mathf.Round(AllPoint).ToString();
        }
    }
}
