using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour
{
    private static Items instance;
    public static Items MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Items>();
            }
            return instance;
        }
    }

    public Text itemName;
    public Image theItemPreview;
    public Image Equipped;
    public Image Locked;
    [SerializeField]
    private Image Speed;
    public Text SpeedValue;
    [SerializeField]
    private Image Weight;
    public Text WeightValue;
    [SerializeField]
    private Image PointShot;
    public Text PointShotValue;
    public Text Price;

    private float currentFill;
    public float MyMaxValue { get; set; }
    private float currentValue;

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {

            if (value > MyMaxValue)
            {
                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;

            }
            currentFill = currentValue / MyMaxValue;
        }

    }

    public void Initialize(float currentValue, float maxValue,Image Stats)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        Stats.fillAmount = MyCurrentValue / MyMaxValue;
    }
}
