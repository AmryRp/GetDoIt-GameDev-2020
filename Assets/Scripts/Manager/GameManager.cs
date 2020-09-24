﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
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
    [SerializeField]
    private UIManager UI;
    [SerializeField]
    private Animator isShowing;
    [SerializeField]
    private PlayerController PC;
    private void Start()
    {
        PC = PlayerController.MyPlayerControl;
        UI = UIManager.MyUI;
        isShowing = GameObject.FindGameObjectWithTag("ShowImage").GetComponent<Animator>();
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

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
        {
            if (UI = null)
            {
                UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
            }
            else { UI = UIManager.MyUI; }
            
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                SceneManager.LoadScene("MainMenu");
            }
            else 
            {
                UI.LoadUI(false, false, false, false, false, true, false);
            }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            PC.IsAnimator.SetBool("IsCapture", true);
            PC.IsAnimator.SetBool("IsMoving", false);
            PC.IsAnimator.SetBool("IsStop", false);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            UI.LoadUI(true, false, false, false, false, false, false);
            isShowing.SetBool("ShowImage", false);
            Time.timeScale = 1f;
        }
    }
    public IEnumerator PauseTime()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
    }
    public bool AudioIsPlay;
    public bool sfxIsPlay;
    public bool isCapturing;

}
