using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class ShoppingListManager : MonoBehaviour
{
    public GameObject ItemPrefab;
    [Header("Meta")]
    //private string json;
    public string SAVE_FILE;
    [Header("Scriptable Objects")]
    public List<ItemServices> itemServicesList;
    public Text[] CoinsShop;
    public Image[] CanoeImageStatic;
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
    //for saving the data
    public void saveItemShop()
    {
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            PlayerPrefs.SetString("", itemServicesList[i].ItemID);
        }
    }
    //for updating the UI view
    public void updateItemShop()
    {
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            PlayerPrefs.SetString("", itemServicesList[i].ItemID);
        }
    }
    //for update bought status in click.
    public void updateBpught(string ItemID)
    {
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            if (itemServicesList[i].ItemID == ItemID)
            {
                itemServicesList[i].locked = true;
                itemServicesList[i].bought = true;
            }
            //PlayerPrefs.SetString("", itemServices[i].ItemID);

        }
    }
    public void DeletePrefabs()
    {
        GameObject[] ListObject = GameObject.FindGameObjectsWithTag("ShopItem");
        for (int i = 0; i < ListObject.Length; i++)
        {
            Destroy(ListObject[i]);
        }
    }
    private List<ItemServices> ItemServicesListJson()
    {
        //load the data from Json

        return itemServicesList;
    }

    public void loadItemShop()
    {
        //load the data from json with json reader input into itemsServices List

        //itemServicesList = ItemServicesListJson();
        
        // i will gonna replace this data to load from json
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            //add new ItemShop
            myItemsPreview = Instantiate(ItemPrefab, ShoppingListManager.MyInstance.transform).GetComponent<Items>();
            //add Init Name,Price,ItemPreview
            myItemsPreview.ItemID = i.ToString();
            myItemsPreview.itemName.text = itemServicesList[i].itemName;
            myItemsPreview.Price.text = itemServicesList[i].price.ToString();
            myItemsPreview.theItemPreview.sprite = itemServicesList[i].theItemPreview;
            //load Item Stat
            myItemsPreview.SpeedValue.text = itemServicesList[i].speedStat.ToString();
            myItemsPreview.Initialize(itemServicesList[i].speedStat, 10, myItemsPreview.Speed);
            myItemsPreview.WeightValue.text = itemServicesList[i].weightStat.ToString();
            myItemsPreview.Initialize(itemServicesList[i].weightStat, 10, myItemsPreview.Weight);
            myItemsPreview.PointShotValue.text = itemServicesList[i].pointValueStat.ToString();
            myItemsPreview.Initialize(itemServicesList[i].pointValueStat, 10, myItemsPreview.PointShot);
            //load Image equipped
            myItemsPreview.Equipped.GetComponent<Image>().enabled = itemServicesList[i].equiped;
            //load Locked Icon
            myItemsPreview.Locked.SetActive(itemServicesList[i].locked);

            if (myItemsPreview.Equipped.enabled)
            {
                myItemsPreview.SelectedItem.color = itemServicesList[i].purchased;

            }
            if (itemServicesList[i].bought == true)
            {
                myItemsPreview.BgColor.color = itemServicesList[i].purchased;
                myItemsPreview.bought.interactable = false;
                myItemsPreview.HidePrice.SetActive(false);
                //load animation
                myItemsPreview.Locked.SetActive(false);
            }
            else
            {
                myItemsPreview.BgColor.color = itemServicesList[i].notpurchased;
                myItemsPreview.bought.interactable = true;
                myItemsPreview.HidePrice.SetActive(true);
                myItemsPreview.Locked.SetActive(true);
            }

        }
        //save the data
        SaveToJsonSaveFile();
    }
    public void SaveToJsonSaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.psv", SAVE_FILE));
        var json = "";
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            json += JsonUtility.ToJson(itemServicesList[i]);
        }
        bf.Serialize(file, json);
        file.Close();
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
        MyCoins--; //Kurangi Point by 1
        for (int i = 0; i < CoinsShop.Length; i++)
        {
            CoinsShop[i].text = Mathf.Round(Mathf.MoveTowards(savePoint, MyCoins, 0.1f * Time.unscaledDeltaTime)).ToString();
            yield return null;
        }
        //make it wait till animation of unlocked done
        DeletePrefabs();
        //load the new list
        loadItemShop();
        //update the saved data to json again
        SaveToJsonSaveFile();
    }


}
