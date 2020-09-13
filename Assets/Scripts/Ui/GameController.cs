using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : UiController, IPointerClickHandler
{
    public string ModeName;
    public string ButtonName;
    
   
    public void OnPointerClick(PointerEventData eventData)
    {
      
        switch (ButtonName)
        {
            case "SceneChange":
                LoadPlay(ModeName);
                break;
            case "PauseButton":
                PauseButton();
                break;
            case "BackButton":
                print("Time Mode");
                break;
            case "Challenge":
                print("unknown");
                break;
            case "Setting":
                print("unknown");
                break;
            case "Gallery":
                print("unknown");
                break;
            case "PLayerInfo":
                print("unknown");
                break;
            default:
                print("Incorrect button Name");
                break;
        }
    }
    public void PauseButton() 
    {
        print("PAUSED");
    }
    public void LoadPlay(string Name)
    {

        SceneManager.LoadScene(Name, LoadSceneMode.Single);
    }
}
