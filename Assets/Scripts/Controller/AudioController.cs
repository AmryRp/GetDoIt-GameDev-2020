using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    private static AudioController instance;
    public static AudioController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioController>();
            }
            return instance;
        }
    }


    public static AudioClip CanoeDrop, CanoeHit, DayungR, CaptureShutter, sfxSec;
    [SerializeField]
    public static AudioSource audios;

    [SerializeField]
    private Slider VolumeBGM;
    [SerializeField]
    private Toggle[] ToggleBGM;
    [SerializeField]
    private Slider VolumeSFX;
    [SerializeField]
    private Toggle[] ToggleSFX;
    [Header("LIST ")]
    [SerializeField]
    public GameObject[] BGM;
    [SerializeField]
    public GameObject[] SFX;

    public GameObject[] BGM1 { get => BGM; set => BGM = value; }
    public GameObject[] SFX1 { get => SFX; set => SFX = value; }

    private void Start()
    {
        ambilsource();
        if (VolumeBGM == null || VolumeSFX == null || ToggleBGM == null || ToggleSFX == null)
        {
            VolumeBGM = GameObject.FindGameObjectWithTag("SliderBGM").GetComponent<Slider>();
            VolumeSFX = GameObject.FindGameObjectWithTag("SliderSFX").GetComponent<Slider>();
            for (int i = 0; i < ToggleBGM.Length; i++)
            {
                ToggleBGM[i] = GameObject.FindGameObjectsWithTag("ToggleBGM")[i].GetComponent<Toggle>();
                ToggleSFX[i] = GameObject.FindGameObjectsWithTag("ToggleSFX")[i].GetComponent<Toggle>();
            }

        }
        VolumeBGM.onValueChanged.AddListener(BGMValue);
        VolumeSFX.onValueChanged.AddListener(SFXValue);
        for (int i = 0; i < ToggleBGM.Length; i++)
        {
            ToggleBGM[i].onValueChanged.AddListener(BGMToggle);
            ToggleSFX[i].onValueChanged.AddListener(SFXToggle);
            ToggleBGM[i].isOn = PlayerPrefs.GetInt("BGM") == 1 ? true : false;
            ToggleSFX[i].isOn = PlayerPrefs.GetInt("SFX") == 1 ? true : false;
        }
        UpdateVol();
        UpdateSetting();
    }
    public void UpdateVol()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            VolumeBGM.value = PlayerPrefs.GetFloat("MusicVolume");
        }
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            VolumeSFX.value = PlayerPrefs.GetFloat("SFXVolume");
        }
    }
    public void UpdateSetting()
    {

        for (int i = 0; i < ToggleBGM.Length; i++)
        {
            if (PlayerPrefs.HasKey("BGM"))
            {
                if (PlayerPrefs.GetInt("BGM").Equals(1))
                {
                    ToggleBGM[i].isOn = true;
                }
                else 
                {
                    ToggleBGM[i].isOn = false;

                    for (int j = 0; j < BGM1.Length; j++)
                    {

                        BGM1[j].GetComponent<AudioSource>().Stop();

                    }
                }

            }
            else
            {
                ToggleBGM[i].isOn = true;
            }
        }
        for (int i = 0; i < ToggleBGM.Length; i++)
        {
            if (PlayerPrefs.HasKey("SFX"))
            {
                if (PlayerPrefs.GetInt("SFX").Equals(1))
                {
                    ToggleSFX[i].isOn = true;
                }
                else
                {
                    ToggleSFX[i].isOn = false;
                    for (int j = 0; j < SFX1.Length; j++)
                    {

                        SFX1[j].GetComponent<AudioSource>().Stop();

                    }
                }
            }
            else
            {
                ToggleSFX[i].isOn = true;
            }
        }

    }
    public void updateList()
    {
        BGM1 = GameObject.FindGameObjectsWithTag("BGM");
        SFX1 = GameObject.FindGameObjectsWithTag("SFX");
        if (BGM1.Length != 0)
        {
            if (BGM1 != null)
            {

                for (int i = 0; i < BGM1.Length; i++)
                {

                    BGM1[i].GetComponent<AudioSource>().volume = VolumeBGM.value;

                }
                //for (int i = 0; i < ToggleBGM.Length; i++)
                //{
                //    ToggleBGM[i].isOn = PlayerPrefs.GetInt("BGM") == 1 ? true : false;
                //}
            }
        }
        if (SFX1.Length != 0)
        {
            if (SFX1 != null)
            {
                for (int i = 0; i < SFX1.Length; i++)
                {

                    SFX1[i].GetComponent<AudioSource>().volume = VolumeSFX.value;
                }
                //for (int i = 0; i < ToggleBGM.Length; i++)
                //{
                //    ToggleSFX[i].isOn = PlayerPrefs.GetInt("SFX") == 1 ? true : false;
                //}

            }
        }

    }

    public void BGMValue(float value)
    {

        for (int i = 0; i < BGM1.Length; i++)
        {

            BGM1[i].GetComponent<AudioSource>().volume = VolumeBGM.value;

        }
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
    public void SFXValue(float value)
    {
        for (int i = 0; i < SFX1.Length; i++)
        {
            SFX1[i].GetComponent<AudioSource>().volume = VolumeSFX.value;
        }
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();

    }

    public void BGMToggle(bool value)
    {
        for (int i = 0; i < 2; i++)
        {
            ToggleBGM[i].isOn = true ? false : true;
            
        }
        value = ToggleBGM[1].isOn;
        print("B" + value);
        int prefs = 0;
        if (value == false ? false : true)
        {
            for (int i = 0; i < BGM1.Length; i++)
            {
                BGM1[i].GetComponent<AudioSource>().Stop();
                prefs = 0;
            }

        }
        else
        {
            for (int i = 0; i < BGM1.Length; i++)
            {
                BGM1[i].GetComponent<AudioSource>().Play();
                prefs = 1;
            }

        }
        PlayerPrefs.SetInt("BGM", prefs);
        PlayerPrefs.Save();
        UpdateSetting();
    }
    public void SFXToggle(bool value)
    {
        for (int i = 0; i < 2; i++)
        {
            ToggleSFX[i].isOn = true ? false : true;
        }
        value = ToggleBGM[1].isOn;
        print("B" + value);
        int prefs = 0;
        if (value == false ? false : true)
        {
            for (int i = 0; i < SFX1.Length; i++)
            {
                SFX1[i].GetComponent<AudioSource>().Stop();
                SFX1[i].GetComponent<AudioSource>().mute = true;
                prefs = 0;
            }

        }
        else
        {
            for (int i = 0; i < SFX1.Length; i++)
            {
                SFX1[i].GetComponent<AudioSource>().Play();
                SFX1[i].GetComponent<AudioSource>().mute = false;
                prefs = 1;
            }
            //PlayerPrefs.SetInt("SFX", ToggleSFX.isOn ? 1 : 0);
            //PlayerPrefs.Save();
        }
        PlayerPrefs.SetInt("SFX", prefs);
        PlayerPrefs.Save();
        UpdateSetting();
    }
    void Update()
    {

        updateList();

    }

    public void ambilsource()
    {
        audios = GetComponent<AudioSource>();
        CanoeDrop = Resources.Load<AudioClip>("Sounds/SFX/CanoeDrop");
        CanoeHit = Resources.Load<AudioClip>("Sounds/SFX/cklick");
        DayungR = Resources.Load<AudioClip>("Sounds/SFX/Dayung2R");
        CaptureShutter = Resources.Load<AudioClip>("Sounds/SFX/Tittitcekrek");
        //sfxSec = Resources.Load<AudioClip>("boom");


    }
    public static void Playsound(string clip)
    {
        switch (clip)
        {
            case "DropSound":
                audios.PlayOneShot(CanoeDrop);
                break;
            case "HitObstacle":
                audios.PlayOneShot(CanoeHit);
                break;
            case "Move":
                audios.PlayOneShot(DayungR);
                break;
            case "Jepret":
                audios.PlayOneShot(CaptureShutter);
                break;
            case "Null":
                audios.PlayOneShot(sfxSec);
                break;

        }
    }
}