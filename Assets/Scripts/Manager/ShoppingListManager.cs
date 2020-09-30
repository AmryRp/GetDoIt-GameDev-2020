using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingListManager : MonoBehaviour
{
    public GameObject ItemPrefab;
    public List<ItemServices> itemServices;
    public Text[] CoinsShop;
    public Items myItemsPreview { get; set; }
    private static ShoppingListManager instance;
    public static ShoppingListManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ShoppingListManager>();
            }
            return instance;
        }
    }


    private void Awake()
    {
        loadItemShop();
        float str = PlayerPrefs.GetFloat("MyPoint");
        if (str.Equals(0f))
        {
            PlayerPrefs.SetFloat("MyPoint", 100f);
            PlayerPrefs.Save();
        }
        else
        {
            for (int i = 0; i < CoinsShop.Length; i++)
            {
                CoinsShop[i].text = str.ToString();
            }
        }
    }

    public void loadItemShop()
    {
        for (int i = 0; i < itemServices.Count; i++)
        {
            //add new ItemShop
            myItemsPreview = Instantiate(ItemPrefab, ShoppingListManager.MyInstance.transform).GetComponent<Items>();
            //add Init Name,Price,ItemPreview
            myItemsPreview.ItemID = i.ToString();
            myItemsPreview.itemName.text = itemServices[i].itemName;
            myItemsPreview.Price.text = itemServices[i].price.ToString();
            myItemsPreview.theItemPreview.sprite = itemServices[i].theItemPreview;

            //load Item Stat
            myItemsPreview.SpeedValue.text = itemServices[i].speedStat.ToString();
            myItemsPreview.Initialize(itemServices[i].speedStat, 10, myItemsPreview.Speed);
            myItemsPreview.WeightValue.text = itemServices[i].weightStat.ToString();
            myItemsPreview.Initialize(itemServices[i].weightStat, 10, myItemsPreview.Weight);
            myItemsPreview.PointShotValue.text = itemServices[i].pointValueStat.ToString();
            myItemsPreview.Initialize(itemServices[i].pointValueStat, 10, myItemsPreview.PointShot);
            //load Image equipped
            myItemsPreview.Equipped.GetComponent<Image>().enabled = itemServices[i].equiped;
            if (myItemsPreview.Equipped.enabled)
            {
                myItemsPreview.SelectedItem.color = itemServices[i].purchased;
            }
            //load Locked Icon
            myItemsPreview.Locked.enabled = itemServices[i].locked;
            //load 
        }
    }
    public void Decrease(float Price)
    {
        StartCoroutine(DecreaseCoin(Price));
    }
    public IEnumerator DecreaseCoin(float decrease)
    {
        float tmpCoins = 0f;
        float MyCoins = PlayerPrefs.GetFloat("MyPoint");
        float str = PlayerPrefs.GetFloat("MyPoint");
        if (!str.Equals(0f))
        {
            tmpCoins = str - decrease;
            PlayerPrefs.SetFloat("MyPoint", tmpCoins);
            PlayerPrefs.Save();
        }
        float savePoint = tmpCoins;

        while (MyCoins > savePoint)
        {
            MyCoins--; //Kurangi Point by 1
            for (int i = 0; i < CoinsShop.Length; i++)
            {
                CoinsShop[i].text = Mathf.Round(Mathf.MoveTowards(savePoint, MyCoins, 0.1f * Time.unscaledDeltaTime)).ToString();
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.1f);

    }
}
