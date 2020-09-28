using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemServices : IBuyAble<int>,IChangeable<string>
{

    private const bool truestate = true; 
    private const bool falsestate = false;
    public string itemName;
    public string price;
    public Image theItemPreview;
    public Sprite itemIcon;
    public bool equiped = falsestate;
    public Image equipedImage;
    public bool bought = falsestate;
    public static Color purchased = Color.blue;
    public static Color notpurchased = Color.white;
    public bool locked = truestate;
    public int discount;
    public int speedStat;
    public int weight;
    public int pointValue;


    public void Buy(int Type)
    {
        
    }

    public void Change(string Type)
    {
        
    }
    

}
