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
    
    public string itemName;
    public string ItemID;
    public int price;
    public Sprite theItemPreview;
    public Sprite itemIcon;
    public bool equiped = falsestate;
    public Image equipedImage;
    public bool bought = falsestate;
    public Color purchased = Color.blue;
    public Color notpurchased = Color.white;
    public bool locked = truestate;
    public int discount;
    public float speedStat;
    public float weightStat;
    public float pointValueStat;

    public string Buy(string Type)
    {
        Type = Items.MyInstance.ItemID;
        float price = float.Parse(Items.MyInstance.Price.text);
        ShoppingListManager.MyInstance.Decrease(price);
        return Type;
    }

    public string Change(string Type)
    {
        Type = ItemID;
        return Type;
    }
}
