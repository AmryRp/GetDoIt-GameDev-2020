using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingListManager : MonoBehaviour
{
    public GameObject ItemPrefab;
    public List<ItemServices> items;
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
 

    private void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            
            myItemsPreview = Instantiate(ItemPrefab, ShoppingListManager.MyInstance.transform).GetComponent<Items>();
            myItemsPreview.itemName.text = items[i].itemName;
        }
       
       
    }

}
