﻿using System;
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
    public string ItemID;
    public Text itemName;
    public Image theItemPreview;
    public Toggle Equipped;
    public GameObject Locked;
    public Image Speed;
    public Text SpeedValue;
    public Image Weight;
    public Text WeightValue;
    public Image PointShot;
    public Text PointShotValue;
    public Text Price;
    public Image SelectedItem;
    public Button bought;
    public GameObject HidePrice;
    public Image BgColor;


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
        Stats.fillAmount = currentFill;
       
    }
    public IEnumerator Unlocked()
    {
        Animator UnLockAnim = Locked.gameObject.GetComponent<Animator>();
        UnLockAnim.SetBool("Unlock", true);
        yield return null;
    }
    public void buyButtonAct()
    {
        float val = float.Parse(Price.text);
        ShoppingListManager.MyInstance.Decrease(val);
        Debug.Log(ItemID);
        Debug.Log(val);
        ShoppingListManager.MyInstance.updateBpught(ItemID);
        Locked.GetComponent<Image>().enabled = false;
    }
    public void Start()
    {
        Equipped = GetComponentInChildren<Toggle>();
        Equipped.onValueChanged.AddListener(EquipButton);
    }
    public void EquipButton(bool value)
    {
        value = Equipped.isOn;
        if (value ? false : true)
        {
            Debug.Log(ItemID + " Is not Equipped " + value);
            ShoppingListManager.MyInstance.EquipItems(ItemID,value);
        }
        else
        {
            Debug.Log(ItemID + " Is Equipped " + value);
            ShoppingListManager.MyInstance.EquipItems(ItemID, value);
        }

       
       
    }
}
