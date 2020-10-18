using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalerryView : MonoBehaviour
{
    private static GalerryView instance;
    public static GalerryView MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GalerryView>();
            }
            return instance;
        }
    }
    public int PhotoId;
    public Text PhotoName;
    public Image PhotoMiniPreview;
    public bool sharedPhotos;
    public void buttonView()
    {
        Image PhotoViewer = GameObject.FindGameObjectWithTag("PhotoView").GetComponent<Image>();
        PhotoViewer.sprite = PhotoMiniPreview.sprite;
        Text PhotoViewName = GameObject.FindGameObjectWithTag("PhotoView").GetComponentInChildren<Text>();
        PhotoViewName.text = PhotoName.text;
    }

}
