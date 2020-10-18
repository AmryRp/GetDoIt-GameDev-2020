using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
public class GalleryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PhotoPrefabs;
    [SerializeField]
    private List<PhotoServices> PhotosLoaded;
    public Text DistanceText, ShotTakenText;
    public GalerryView myPhotoPreview { get; set; }

    private static GalleryManager instance;
    public static GalleryManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GalleryManager>();
            }
            return instance;
        }
    }
    private string GetAndroidExternalStoragePath()
    {
        if (Application.platform != RuntimePlatform.Android)
            return Application.persistentDataPath;
        var jc = new AndroidJavaClass("android.os.Environment");
        var path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
            jc.GetStatic<string>("DIRECTORY_DCIM"))
            .Call<string>("getAbsolutePath");
        return path;
    }
    void Start()
    {
        float str2 = PlayerPrefs.GetFloat("DistanceTraveled");
        int str3 = PlayerPrefs.GetInt("MyShot");

        DistanceText.text = Mathf.Round(str2).ToString();
        ShotTakenText.text = str3.ToString();

        BinaryFormatter bf = new BinaryFormatter();
        DirectoryInfo dir = new DirectoryInfo(GetAndroidExternalStoragePath() + "/" + Application.productName + " Captures");
        if (!Directory.Exists(GetAndroidExternalStoragePath() + "/" + Application.productName + " Captures"))
        {
            var folder = Directory.CreateDirectory(GetAndroidExternalStoragePath() + "/" + Application.productName + " Captures");
        }

        var files = dir.GetFiles().Where(o => o.Name.EndsWith(".png")).ToArray();
        for (int i = 0; i < files.Length; i++)
        {
            float PixelsPerUnit = 100.0f;
            SpriteMeshType spriteType = SpriteMeshType.Tight;
            Texture2D SpriteTexture = LoadTexture(files[i].FullName);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
            PhotoServices imagefile = new PhotoServices();
            imagefile.photosTexture = NewSprite;
            imagefile.photoId = i;
            imagefile.photosName = files[i].Name.ToString();
            imagefile.sharedPhotos = false;
            PhotosLoaded.Add(imagefile);
            myPhotoPreview = Instantiate(PhotoPrefabs, GalleryManager.MyInstance.transform).GetComponent<GalerryView>();
            //AllFiles[i] = (FileModel)bf.Deserialize(fs);
            myPhotoPreview.PhotoMiniPreview.sprite = PhotosLoaded[i].photosTexture;
            myPhotoPreview.PhotoName.text = PhotosLoaded[i].photosName.Replace(".png", "");
            myPhotoPreview.PhotoId = PhotosLoaded[i].photoId;
            myPhotoPreview.sharedPhotos = PhotosLoaded[i].sharedPhotos;
        }


    }
    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
