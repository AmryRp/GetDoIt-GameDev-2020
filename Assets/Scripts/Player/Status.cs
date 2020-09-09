using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    private static Status instance;
    public static Status MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Status>();
            }
            return instance;
        }
    }
    private Image content;

    [SerializeField]
    public Text statValue;
    private float currentFill;

    private float overExp;

    [SerializeField]
    private float lerpSpeed = 0;

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
                overExp = value - MyMaxValue;
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
          
            if (statValue != null)
            {
                statValue.text = Mathf.Round( currentValue) + "  /  " + MyMaxValue;
            }
        }

    }
    public bool ExpFull
    {
        get
        {
            return content.fillAmount == 1;
        }
    }
    public float MyOverExp
    {
        get
        {
            float tmp = overExp;
            overExp = 0;
            return tmp;

        }
    }

    void Start()
    {

        content = GetComponent<Image>();

    }
    private void Update()
    {
        HandleBar();
    }


    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();

        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void HandleBar()
    {

        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.time * lerpSpeed);
        }
    }
    public void Reset()
    {
        content.fillAmount = 0;
    }
}
