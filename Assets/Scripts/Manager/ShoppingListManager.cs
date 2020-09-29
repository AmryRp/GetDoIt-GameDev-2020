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
                CoinsShop[i].text =  str.ToString();
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
            myItemsPreview.ItemID = itemServices[i].ItemID;
            myItemsPreview.itemName.text = itemServices[i].itemName;
            myItemsPreview.Price.text = itemServices[i].price.ToString();
            myItemsPreview.theItemPreview.sprite = itemServices[i].theItemPreview;

            //load Item Stat
            myItemsPreview.SpeedValue.text = itemServices[i].speedStat.ToString();
            myItemsPreview.Speed.fillAmount = (itemServices[i].speedStat / 10);
            myItemsPreview.WeightValue.text = itemServices[i].weightStat.ToString();
            myItemsPreview.Weight.fillAmount = (itemServices[i].weightStat / 10);
            myItemsPreview.PointShotValue.text = itemServices[i].pointValueStat.ToString();
            myItemsPreview.PointShot.fillAmount = (itemServices[i].pointValueStat / 10);
            //load Image equipped

            //load Locked Icon

            //load 
        }
    }
    public void BuyButton()
    {
        for (int i = 0; i < itemServices.Count; i++)
        {
            itemServices[i].Buy(itemServices[i].itemName);
        }

    }
    public IEnumerator DecreaseCoin(float decrease)
    {
        float tmpCoins = PlayerPrefs.GetInt("MyPoint") - decrease;

        float MyCoins = PlayerPrefs.GetInt("MyPoint");
        while (true)
        {
            if (MyCoins >= tmpCoins)
            {
                MyCoins--; //Kurangi Point by 1
                for (int i = 0; i < CoinsShop.Length; i++)
                {
                    CoinsShop[i].text = Mathf.Round(Mathf.Lerp(tmpCoins, MyCoins, 0.1f * Time.deltaTime)).ToString();
                }
            }
            Debug.Log(MyCoins + " - " + tmpCoins);
            PlayerPrefs.SetFloat("MyPoint", MyCoins);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
