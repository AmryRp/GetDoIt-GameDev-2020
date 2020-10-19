using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Text timerText;
    public float timeCountdownInSecond = 90f;
    public UIManager UI;

    private float timer;
    public bool isTimeUp;

    // Start is called before the first frame update
    void Start()
    {
        isTimeUp = true;
        SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            timer = 0;
            if (timeCountdownInSecond > 0)
            {
                timeCountdownInSecond--;
                if (timeCountdownInSecond == 10) timerText.color = Color.red;
                SetTimer();
            }
            else
            {
                Debug.Log("Time is Up = " + isTimeUp);
                timer = 0;
                Debug.Log(isTimeUp);
                if (isTimeUp)
                {
                    TimeUp();
                }
            }
        }
    }

    private void TimeUp()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        Time.timeScale = 1f;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        StartCoroutine(UI.CalculatingPrefabPoint());
        UI.LoadUI(false, false, false, false, false, false, true, false, false, false);
        isTimeUp = false;        
        Debug.Log("Time is Up2 = " + isTimeUp);
    }

    private void SetTimer()
    {
        string minutes = Mathf.Floor(timeCountdownInSecond / 60).ToString();
        string seconds = Mathf.Floor(timeCountdownInSecond % 60).ToString();
        timerText.text = minutes + " : " + seconds;
    }
}
