using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class AfterAnimationController : MonoBehaviour
{
    UIManager UI;
    public void StopTimer()
    {
        Time.timeScale = 0f;
    }
    public void OpenCapture()
    {
        if (UI = null)
        {
            UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        }
        UI = UIManager.MyUI;
        UI.LoadUI(false, false, false, false, true, false, false,false);
        StartCoroutine(PlayAnim());
    }
    public IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(2);
        Animator isShowing = GameObject.FindGameObjectWithTag("ShowImage").GetComponent<Animator>();
        isShowing.SetBool("ShowImage", true);
    }
    public void sceneOneloader()
    {
        //load the scene we want
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }
    public void sceneMenuloader()
    {
        //load the scene we want
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void sceneTimeModeloader()
    {
        //load the scene we want
        SceneManager.LoadScene("T", LoadSceneMode.Single);
    }
    public void playSFX(string Name)
    {
        AudioController.Playsound(Name);
    }
}
