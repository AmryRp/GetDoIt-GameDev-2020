using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class AfterAnimationController : MonoBehaviour
{
    [SerializeField]
    UIManager UI;
    [SerializeField]
    PlayerController PL;
    [SerializeField]
    GameManager GM;
    public void StopTimer()
    {
        Time.timeScale = 0f;
    }
    public void OpenCapture()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        UI.LoadUI(false, false, false, false, true, false, false, false, false, false);
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
        SceneManager.LoadScene("LevelTimerMode", LoadSceneMode.Single);
    }
    public void playSFX(string Name)
    {
        AudioController.Playsound(Name);
    }
    float multiplier;
    Animator animmove;
    public void PlayerMoveFixed()
    {
        PL = PlayerController.MyPlayerControl;
        GM = GameManager.MyGM;
        // StartCoroutine(PL.MovePlayer(PL.MoveSpeedInWater));

        //animmove = PL.GetComponentInChildren<Animator>();

        //if (!PL.speedLimiter(animmove.GetFloat("MoveMultiplier")))
        //{
        //    print("jalan");
        //    multiplier += 1f;
        //    animmove.SetFloat("MoveMultiplier", multiplier);
        //}

        //if (!PL.is_hold && PL.Hidup && !GM.isCapturing )
        //{
        //    StartCoroutine(PL.MovePlayer(PL.MoveSpeedInWater));
        //}
        //else
        //{
        //    animmove.SetBool("IsIdle", true);
        //    animmove.SetBool("IsMoving", false);
        //}

    }
    public void PlayerResetMovemnent()
    {
        PL = PlayerController.MyPlayerControl;
        StartCoroutine(PL.StartIdling());
        if (PL.IdleTimer >= 3f)
        {
            // print("idling");
            animmove = PL.GetComponentInChildren<Animator>();
            multiplier = 1;
            animmove.SetFloat("MoveMultiplier", multiplier);
            PL.IdleTimer = 0f;
        }
    }
}
