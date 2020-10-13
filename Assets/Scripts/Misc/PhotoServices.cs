using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PhotoServices 
{
    //private const bool truestate = true;
    private const bool falsestate = false;
    [SerializeField]
    public int photoId;
    [SerializeField]
    public String photosName;
    [SerializeField]
    public bool sharedPhotos = falsestate;
    [SerializeField]
    public Sprite photosTexture;

    //public Sprite PhotosTexture { get => photosTexture; set => photosTexture = value; }
    //public bool SharedPhotos { get => sharedPhotos; set => sharedPhotos = value; }
    //public int PhotoId { get => photoId; set => photoId = value; }
    //public string PhotosName { get => photosName; set => photosName = value; }

    // Start is called before the first frame update
}
