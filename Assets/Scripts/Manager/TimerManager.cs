using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Text timerText;
    public Text millisecondsText;
    public float timeCountdownInSecond = 90f;
    public UIManager UI;

    private float timer = 1;
    public bool isTimeUp;
    private string minutes, seconds, milliseconds;

    // Start is called before the first frame update
    void Start()
    {
        isTimeUp = true;
        SetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        SetTimer();
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
        timer -= Time.deltaTime;
        if (timeCountdownInSecond > -1)
        {
            GetTime(timer); // Hitung time
            
            if (timer <= 0)
            {
                timer = 1;
            }
        }
        else
        {
            timer = 0;
            Debug.Log(isTimeUp);
            if (isTimeUp)
            {
                TimeUp();
            }
        }
    }

    private void GetTime(float timer)
    {
        if (timer > 0)
        {
            milliseconds = (Mathf.Floor(timer * 100f) % 100).ToString();
            millisecondsText.text = milliseconds; //Set milisecond
            return;
        } else
        {
            timeCountdownInSecond--;
            minutes = Mathf.Floor(timeCountdownInSecond / 60).ToString();
            seconds = Mathf.Floor(timeCountdownInSecond % 60).ToString();

            int inSeconds = int.Parse(seconds);

            if(inSeconds < 10)
            {
                seconds = "0" + seconds;
            }
            
            if (timeCountdownInSecond == 10)
            {
                timerText.color = Color.red;
                millisecondsText.color = Color.red;
            }

            timerText.text = minutes + " : " + seconds + " : "; // Set Minutes and Seconds
        }
    }
}
