using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemServices : IBuyAble<string>,IChangeable<string>
{

    private const bool truestate = true; 
    private const bool falsestate = false;
    public string ItemID;
    public string itemName;
    public int price;
    public Sprite theItemPreview;
    public Sprite itemIcon;
    public bool equiped = falsestate;
    public Image equipedImage;
    public bool bought = falsestate;
    public static Color purchased = Color.blue;
    public static Color notpurchased = Color.white;
    public bool locked = truestate;
    public int discount;
    public int speedStat;
    public int weightStat;
    public int pointValueStat;

    public string Buy(string Type)
    {
        Type = ItemID;
        return Type;
    }

    public string Change(string Type)
    {
        Type = ItemID;
        return Type;
    }
}
