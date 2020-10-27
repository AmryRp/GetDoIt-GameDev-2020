using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    private static ItemView instance;
    public static ItemView MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ItemView>();
            }
            return instance;
        }
    }
    public int ItemID;
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
    public GameObject HideEquip;
    public Text EquipText;

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
    public void buyButtonAct(string CanoeOrCoin)
    {
        try
        {
            if (Price.text.Split("Rp."[2])[1] != null)
            {
                CanoeOrCoin = Price.text.Split("Rp."[2])[0];
                print(Price.text);
                print(Price.text.Split("Rp."[2])[0]);
                print(Price.text.Split("Rp."[2])[1]);
                SSTools.ShowMessage("Not Implemented Yet", SSTools.Position.bottom, SSTools.Time.oneSecond);
            }
        }
        catch
        {
            float val = float.Parse(Price.text);

            if (ShoppingListManager.MyInstance.Decrease(val))
            {
                Locked.GetComponent<Image>().enabled = false;
                ShoppingListManager.MyInstance.updateBpught(ItemID);
            }
            else 
            {
                SSTools.ShowMessage(" Not Enough Coins ", SSTools.Position.bottom, SSTools.Time.twoSecond);
            }
        }
    }
    public void Start()
    {
        Equipped = HideEquip.GetComponent<Toggle>();
        Equipped.onValueChanged.AddListener(EquipButton);
    }
    public void EquipButton(bool value)
    {
        value = Equipped.isOn;
        if (value ? false : true)
        {
            ShoppingListManager.MyInstance.EquipItems(ItemID, value);
        }
        else
        {
            ShoppingListManager.MyInstance.EquipItems(ItemID, value);
        }



    }
}
