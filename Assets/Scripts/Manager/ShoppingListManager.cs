using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class ShoppingListManager : MonoBehaviour
{
    [Header("Prefab UI ")]
    public GameObject ItemPrefab;
    [Header("Meta Save File Name")]
    //private string json;
    public string SAVE_FILE;
    [Header("Scriptable Objects")]
    public List<ItemServices> itemServicesList;
    public List<ItemServices> InAppPurchaseList;
    [Header("Coins UI")]
    public Text[] CoinsShop;
    [Header("Canoe Image")]
    public Sprite[] CanoeImageStatic;
    [Header("IAP Image")]
    public Sprite[] IAPImageStatic;
    public ItemView myItemsPreview { get; set; }
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
    private void Start()
    {
        //LoadFromJsonSaveFile();
        LoadCanoeEquipShop();
        float str = PlayerPrefs.GetFloat("MyPoint");
        if (str.Equals(0f))
        {
            PlayerPrefs.SetFloat("MyPoint", 5000f);
            PlayerPrefs.Save();
            str = PlayerPrefs.GetFloat("MyPoint");
            loadPointText(str);
        }
        else
        {
            //debug clearing errorpoint
            //PlayerPrefs.DeleteKey("MyPoint");
            loadPointText(str);
        }
    }
    private void loadPointText(float text)
    {
        for (int i = 0; i < CoinsShop.Length; i++)
        {
            CoinsShop[i].text = text.ToString();
        }
    }
    public void switchOption(int indexOption)
    {
        switch (indexOption)
        {
            case 0:
                if (!GameObject.FindGameObjectsWithTag("ShopItem").Length.Equals(0))
                {
                    DeletePrefabs();
                }
                LoadCanoeEquipShop();
                break;
            case 1:
                if (!GameObject.FindGameObjectsWithTag("ShopItem").Length.Equals(0))
                {
                    DeletePrefabs();
                }
                LoadIAP();
                break;
            default:
                print("Incorrect button Name");
                break;
        }

    }
    public void updateBpught(int ItemID)
    {
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            if (itemServicesList[i].ItemID == ItemID)
            {
                itemServicesList[i].locked = true;
                itemServicesList[i].bought = true;
                break;
            }
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
    public void LoadCanoeEquipShop()
    {
        //load the data from json with json reader input into itemsServices List

        //itemServicesList = ItemServicesListJson();
        if (File.Exists(Application.persistentDataPath + string.Format("/{0}.psv", SAVE_FILE)))
        {
            LoadFromJsonSaveFile("psv");
        }
        else
        {
            //add the default data 
            SaveToJsonSaveFile("psv");
        }
        // this data to load from json to UI view
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            //add new ItemShop
            myItemsPreview = Instantiate(ItemPrefab, ShoppingListManager.MyInstance.transform).GetComponent<ItemView>();
            //add Init Name,Price,ItemPreview
            myItemsPreview.ItemID = i;
            myItemsPreview.itemName.text = itemServicesList[i].itemName;
            myItemsPreview.Price.text = itemServicesList[i].price.ToString();
            myItemsPreview.theItemPreview.sprite = CanoeImageStatic[i];
            //load Item Stat
            myItemsPreview.SpeedValue.text = itemServicesList[i].speedStat.ToString();
            myItemsPreview.Initialize(itemServicesList[i].speedStat, 10, myItemsPreview.Speed);
            myItemsPreview.WeightValue.text = itemServicesList[i].weightStat.ToString();
            myItemsPreview.Initialize(itemServicesList[i].weightStat, 10, myItemsPreview.Weight);
            myItemsPreview.PointShotValue.text = itemServicesList[i].pointValueStat.ToString();
            myItemsPreview.Initialize(itemServicesList[i].pointValueStat, 10, myItemsPreview.PointShot);
            //load Locked Icon
            myItemsPreview.Locked.SetActive(itemServicesList[i].locked);

            if (itemServicesList[i].equiped == true)
            {
                //load Image equipped
                myItemsPreview.Equipped.isOn = itemServicesList[i].equiped;
                myItemsPreview.EquipText.text = "EQUIPPED";
            }
            else
            {
                //print(itemServicesList[i].itemName + "NOT ENABLED");
                myItemsPreview.Equipped.isOn = itemServicesList[i].equiped;
                myItemsPreview.EquipText.text = "EQUIP";
            }
            if (itemServicesList[i].bought == true)
            {
                myItemsPreview.BgColor.color = itemServicesList[i].purchased;
                myItemsPreview.bought.interactable = false;
                myItemsPreview.HidePrice.SetActive(false);
                myItemsPreview.HideEquip.SetActive(true);
                //load animation
                myItemsPreview.Locked.SetActive(false);
            }
            else
            {
                myItemsPreview.BgColor.color = itemServicesList[i].notpurchased;
                myItemsPreview.bought.interactable = true;
                myItemsPreview.HidePrice.SetActive(true);
                myItemsPreview.Locked.SetActive(true);
                myItemsPreview.HideEquip.SetActive(false);
            }

        }
    }
    public void LoadIAP()
    {
        //load the data from json with json reader input into itemsServices List

        //itemServicesList = ItemServicesListJson();
        if (File.Exists(Application.persistentDataPath + string.Format("/{0}.iap", SAVE_FILE)))
        {
            LoadFromJsonSaveFile("iap");
        }
        else
        {
            //add the default data 
            SaveToJsonSaveFile("iap");
        }
        // this data to load from json to UI view
        for (int i = 0; i < InAppPurchaseList.Count; i++)
        {
            //add new ItemShop
            myItemsPreview = Instantiate(ItemPrefab, ShoppingListManager.MyInstance.transform).GetComponent<ItemView>();
            //add Init Name,Price,ItemPreview
            myItemsPreview.ItemID = i;
            myItemsPreview.itemName.text = InAppPurchaseList[i].itemName;
            myItemsPreview.Price.text = "Rp." + InAppPurchaseList[i].price.ToString();
            myItemsPreview.theItemPreview.sprite = CanoeImageStatic[i];
            //hidden shop
            myItemsPreview.Locked.GetComponent<Image>().color = Color.black;
            myItemsPreview.Locked.GetComponent<Image>().sprite = IAPImageStatic[0];
            GameObject[] Locker = GameObject.FindGameObjectsWithTag("LockIcon");
            for (int j = 0; j < Locker.Length; j++)
            {
                Locker[j].GetComponent<Image>().sprite = IAPImageStatic[1];
                print(Locker[j]);
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
    public void SaveToJsonSaveFile(string format)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}." + format, SAVE_FILE));
        ItemServices[] JsonArray = new ItemServices[itemServicesList.Count];
        if (format == "psv")
        {
            for (int i = 0; i < itemServicesList.Count; i++)
            {
                JsonArray[i] = itemServicesList[i];
            }
        }
        else
        {
            for (int i = 0; i < InAppPurchaseList.Count; i++)
            {
                JsonArray[i] = InAppPurchaseList[i];
            }
        }

        string playerToJson = JsonHelper.ToJson(JsonArray, true);
        Debug.Log(playerToJson);
        bf.Serialize(file, EncryptJson(playerToJson));
        file.Close();
        //loadItemShop();
    }
    public void LoadFromJsonSaveFile(string format)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}." + format, SAVE_FILE), FileMode.Open);
        ItemServices[] JsonArray = JsonHelper.FromJson<ItemServices>(DecryptJson((string)bf.Deserialize(file)));
        if (format == "psv")
        {
            for (int i = 0; i < itemServicesList.Count; i++)
            {
                itemServicesList[i] = JsonArray[i];
                //print(JsonArray[i].itemName);
                //JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), itemServicesList[i]);
            }
        }
        else
        {
            for (int i = 0; i < InAppPurchaseList.Count; i++)
            {
                InAppPurchaseList[i] = JsonArray[i];
                //print(JsonArray[i].itemName);
                //JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), itemServicesList[i]);
            }
        }
        //bf.Serialize(file, json);
        file.Close();
    }
    //hashcode will used playername too if it can
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
                byte[] dataEncResult = setData.TransformFinalBlock(data, 0, data.Length);
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
    public bool Decrease(float Price)
    { // check if the price listed on list is below the coin that player have
        // if true decrease coin and open the item
        // else error message / toast appear to notify player if the coin is not enough
        bool result = false;
        float MyCoins = PlayerPrefs.GetFloat("MyPoint");
        print(MyCoins);
        if (MyCoins <= Price)
        {
            result = false;
        }
        else
        {
            StartCoroutine(DecreaseCoin(Price));
            result = true;
        }
        return result;
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
        SaveToJsonSaveFile("psv");
        //make it wait till animation of unlocked done
        DeletePrefabs();
        //load the new list
        LoadCanoeEquipShop();
    }
    public void EquipItems(int ID, bool On)
    {
        // StartCoroutine(EquipOneOnly(ID,On));
        for (int i = 0; i < itemServicesList.Count; i++)
        {
            if (itemServicesList[i].ItemID == ID)
            {
                itemServicesList[i].equiped = On;
                //print("equip "+On);
                if (On)
                {
                    print("setvalue");
                    PlayerPrefs.SetInt("CanoeType", itemServicesList[i].ItemID);
                    PlayerPrefs.SetFloat("MyEquipSpeed", itemServicesList[i].speedStat);
                    PlayerPrefs.SetFloat("MyEquipWeight", itemServicesList[i].weightStat);
                    PlayerPrefs.SetFloat("MyEquipPoint", itemServicesList[i].pointValueStat);
                    PlayerPrefs.Save();
                }
                else
                {
                    print("setDefault");
                    PlayerPrefs.SetFloat("MyEquipSpeed", 1f);
                    PlayerPrefs.SetFloat("MyEquipWeight", 1f);
                    PlayerPrefs.SetFloat("MyEquipPoint", 1f);
                    PlayerPrefs.SetInt("CanoeType",-1);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                itemServicesList[i].equiped = false;
            }
        }
        //update the saved data to json again
        SaveToJsonSaveFile("psv");
        //make it wait till animation of unlocked done
        DeletePrefabs();
        //load the new list
        LoadCanoeEquipShop();
    }
    public IEnumerator EquipOneOnly(string Name, bool On)
    {
        yield return null;
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
