using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        UI.LoadUI(false, false, false, false, true, false, false);
        Animator isShowing = GameObject.FindGameObjectWithTag("ShowImage").GetComponent<Animator>();
        isShowing.SetBool("ShowImage", true);
    }
}
