using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : UiController
{
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (!UiControllerxist)
        {
            UiControllerxist = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void LateUpdate()
    {
        
            SwitchScene(SceneManager.GetActiveScene().buildIndex);
       
       
    }
    public void SwitchScene(int SceneName) 
    {
        switch (SceneName)
        {
            case 0:
                Activating(true, false);
                break;
            case 1:
                Activating(false, true);
                break;
            case 2:
                Activating(false, true);
                break;
            case 3:
                print("unknown");
                break;
            default:
                print("Incorrect intelligence level.");
                break;
        }
    }
    public void Activating(bool Menu, bool Gameplay)
    {
        if (!GameManager.MyGM.IsPaused)
        {
            GameObject.FindGameObjectWithTag("MainMenu").GetComponent<Canvas>().enabled = Menu;
            GameObject.FindGameObjectWithTag("GameplayUI").GetComponent<Canvas>().enabled = Gameplay;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
