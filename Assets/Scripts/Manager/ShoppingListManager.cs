using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System;
using System.Security.Cryptography;
public class ShoppingListManager : MonoBehaviour
{
    public GameObject ItemPrefab;
    [Header("Meta")]
    //private string json;
    public string SAVE_FILE;
    [Header("Scriptable Objects")]
    public List<ItemServices> itemServicesList;
    public Text[] CoinsShop;
    public Sprite[] CanoeImageStatic;
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
        //LoadFromJsonSaveFile();
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
    public void loadItemShop()
    {
        //load the data from json with json reader input into itemsServices List

        //itemServicesList = ItemServicesListJson();
        if (File.Exists(Application.persistentDataPath + string.Format("/{0}.psv", SAVE_FILE)))
        {
            LoadFromJsonSaveFile();
        }
        else 
        {
            //add the default data 
            SaveToJsonSaveFile();
        }
        // this data to load from json to UI view
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
    }
    public IEnumerator PlayUnlock(GameObject LockedSprite)
    {
        yield return null;
        Animator UnlockAnim = LockedSprite.GetComponent<Animator>();
        UnlockAnim.SetBool("Unlock", true);
        yield return new WaitForSeconds(1.6f);
    }
    public void SaveToJsonSaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.psv", SAVE_FILE));
        ItemServices[] JsonArray = new ItemServices[itemServicesList.Count];
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            JsonArray[i] = itemServicesList[i];
        }
        string playerToJson = JsonHelper.ToJson(JsonArray, true);
        Debug.Log(playerToJson);
        bf.Serialize(file, EncryptJson(playerToJson));
        file.Close();
     
    }
    public void LoadFromJsonSaveFile()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.psv", SAVE_FILE), FileMode.Open);
        ItemServices[] JsonArray = JsonHelper.FromJson<ItemServices>(DecryptJson((string)bf.Deserialize(file)));
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            itemServicesList[i] = JsonArray[i];
            print(JsonArray[i].itemName);
            //JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), itemServicesList[i]);
        }
        //bf.Serialize(file, json);
        file.Close();
    }
   
    public static string hashcode = "5758184mryg37D0iT";
    public static string EncryptJson(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider Md5first = new MD5CryptoServiceProvider())
        {
            byte[] key = Md5first.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashcode));
            using (TripleDESCryptoServiceProvider Stripper = new TripleDESCryptoServiceProvider()
            { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform setData = Stripper.CreateEncryptor();
                byte[] dataEncResult = setData.TransformFinalBlock(data,0,data.Length);
                return Convert.ToBase64String(dataEncResult, 0, dataEncResult.Length);
            }
        }
    }
    public static string DecryptJson(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider Md5first = new MD5CryptoServiceProvider())
        {
            byte[] key = Md5first.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashcode));
            using (TripleDESCryptoServiceProvider Stripper = new TripleDESCryptoServiceProvider()
            { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform setData = Stripper.CreateDecryptor();
                byte[] dataEncResult = setData.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(dataEncResult);
            }
        }
    }
    //button buy function , check first not implemented yet
    public void Decrease(float Price)
    { // check if the price listed on list is below the coin that player have
        // if true decrease coin and open the item
        // else error message / toast appear to notify player if the coin is not enough

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
        //update the saved data to json again
        SaveToJsonSaveFile();
        //make it wait till animation of unlocked done
        DeletePrefabs();
        //load the new list
        loadItemShop();


    }
}

//Class For Creating Json

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
