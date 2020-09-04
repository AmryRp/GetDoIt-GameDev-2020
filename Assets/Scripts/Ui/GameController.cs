using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    public Button yourButton;

    void Start()
    {
        
        Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(TaskOnClick);
    }

    //void TaskOnClick()
    //{
    //    playerController.MovePlayer();
    //}

}
