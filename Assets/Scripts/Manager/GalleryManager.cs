using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class GalleryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PhotoPrefabs;
    [SerializeField]
    private List<PhotoServices> PhotosLoaded;

    // Start is called before the first frame update
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

        DirectoryInfo dir = new DirectoryInfo(GetAndroidExternalStoragePath()+"/" +Application.productName+" Captures");
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            print(info.Length.ToString());
            SSTools.ShowMessage(info.Length.ToString(), SSTools.Position.bottom, SSTools.Time.twoSecond);
        }
    }
    

}
