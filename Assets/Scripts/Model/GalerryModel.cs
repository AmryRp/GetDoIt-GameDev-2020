using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalerryModel : MonoBehaviour
{
    private static GalerryModel instance;
    public static GalerryModel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GalerryModel>();
            }
            return instance;
        }
    }
    public int PhotoId;
    public Text PhotoName;
    public Image PhotoMiniPreview;
    public Image SelectedItem;
    public Button ClickButton;
    public GameObject HidePrice;
    void Start()
    {
        
    }

}
