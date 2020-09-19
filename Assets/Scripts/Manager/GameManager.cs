using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager MyGM
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private void Start()
    {
        IsPaused = false;
    }
    public bool IsPaused;
    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            IsPaused = true;

        }
        else
        {
            IsPaused = false;
        }

    }
    public bool AudioIsPlay;

}
