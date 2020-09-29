﻿using System.Collections;
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
    public string ItemID;
    public Text itemName;
    public Image theItemPreview;
    public Image Equipped;
    public Image Locked;
    public Image Speed;
    public Text SpeedValue;
    public Image Weight;
    public Text WeightValue;
    public Image PointShot;
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

    public void Initialize(float currentValue, float maxValue, Image Stats)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        Stats.fillAmount = MyCurrentValue / MyMaxValue;
    }
    public IEnumerator Unlocked()
    {
        Animator UnLockAnim = Locked.gameObject.GetComponent<Animator>();
        UnLockAnim.SetBool("Unlock", true);
        yield return null;
    }

}
