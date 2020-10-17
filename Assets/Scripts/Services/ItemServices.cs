using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemServices :  IBuyAble<int>,IChangeable<int>
{

    private const bool truestate = true; 
    private const bool falsestate = false;
    
    public string itemName;
    public int ItemID;
    public int price;               //change image to string / int 
    public Sprite theItemPreview; // get the image static from shopslistmanager
    public Sprite itemIcon;
    public bool equiped = falsestate;
    public Image equipedImage;
    public bool bought = falsestate;
    public Color purchased = Color.blue;
    public Color notpurchased = Color.black;
    public bool locked = truestate;
    public int discount;
    public float speedStat;
    public float weightStat;
    public float pointValueStat;
    public int Buy(int Type)
    {
        Type = ItemView.MyInstance.ItemID;
        float price = float.Parse(ItemView.MyInstance.Price.text);
        ShoppingListManager.MyInstance.Decrease(price);
        return Type;
    }

    public int Change(int Type)
    {
        Type = ItemView.MyInstance.ItemID;
     //   ShoppingListManager.MyInstance.EquipItems(Type);
        return Type;
    }

    public ItemServices()
    {
    }

    public ItemServices(string itemName, int itemID, int price, 
        Sprite theItemPreview, Sprite itemIcon, bool equiped, 
        Image equipedImage, bool bought, Color purchased, 
        Color notpurchased, bool locked, int discount, 
        float speedStat, float weightStat, float pointValueStat)
    {
        this.itemName = itemName;
        ItemID = itemID;
        this.price = price;
        this.theItemPreview = theItemPreview;
        this.itemIcon = itemIcon;
        this.equiped = equiped;
        this.equipedImage = equipedImage;
        this.bought = bought;
        this.purchased = purchased;
        this.notpurchased = notpurchased;
        this.locked = locked;
        this.discount = discount;
        this.speedStat = speedStat;
        this.weightStat = weightStat;
        this.pointValueStat = pointValueStat;
    }


    public override bool Equals(object obj)
    {
        return obj is ItemServices services &&
               itemName == services.itemName &&
               ItemID == services.ItemID &&
               price == services.price &&
               EqualityComparer<Sprite>.Default.Equals(theItemPreview, services.theItemPreview) &&
               EqualityComparer<Sprite>.Default.Equals(itemIcon, services.itemIcon) &&
               equiped == services.equiped &&
               EqualityComparer<Image>.Default.Equals(equipedImage, services.equipedImage) &&
               bought == services.bought &&
               purchased.Equals(services.purchased) &&
               notpurchased.Equals(services.notpurchased) &&
               locked == services.locked &&
               discount == services.discount &&
               speedStat == services.speedStat &&
               weightStat == services.weightStat &&
               pointValueStat == services.pointValueStat;
    }

    public override int GetHashCode()
    {
        var hashCode = -631323268;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(itemName);
        hashCode = hashCode * -1521134295 + EqualityComparer<int>.Default.GetHashCode(ItemID);
        hashCode = hashCode * -1521134295 + price.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(theItemPreview);
        hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(itemIcon);
        hashCode = hashCode * -1521134295 + equiped.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Image>.Default.GetHashCode(equipedImage);
        hashCode = hashCode * -1521134295 + bought.GetHashCode();
        hashCode = hashCode * -1521134295 + purchased.GetHashCode();
        hashCode = hashCode * -1521134295 + notpurchased.GetHashCode();
        hashCode = hashCode * -1521134295 + locked.GetHashCode();
        hashCode = hashCode * -1521134295 + discount.GetHashCode();
        hashCode = hashCode * -1521134295 + speedStat.GetHashCode();
        hashCode = hashCode * -1521134295 + weightStat.GetHashCode();
        hashCode = hashCode * -1521134295 + pointValueStat.GetHashCode();
        return hashCode;
    }
}
