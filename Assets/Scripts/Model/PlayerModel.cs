using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerModel : CharacterModel
{
    [Header("Player Mechanic")]
    [SerializeField]
    protected float PlayerEnergy;
    [SerializeField]
    protected string CanoeType;
    [SerializeField]
    protected float CameraMeter;
    protected static bool playerExists;
    [Header("Player Exp Mechanic")]

    //[SerializeField]
    //protected Status ExpStat;

    //protected Text LevelText;

    public Vector3 lastMove;

    [Header("Player Game Over Canvas UI")]
    [SerializeField]
    public CanvasGroup CGLose;
    protected int scene;

    [Header("Player Energy System")]
    [SerializeField]
    protected float EnergyDrain = 0f;
    protected float batas = 4f;

    private static PlayerModel instance;
    public static PlayerModel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerModel>();
            }
            return instance;
        }
    }

    public PlayerModel(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }

    //public Transform MyTarget { get; set; }
    public Animator IsAnimator { get; set; }

    protected Vector2 direction;

    //protected Rigidbody2D myRigidbody;
    

    protected bool IsOnMove { get; set; }

    protected bool CaptureClick = false;

    [SerializeField]
    protected int Level;
    [SerializeField]
    protected Status Energy;

    [SerializeField]
    protected Text DistanceTraveled;
    [SerializeField]
    protected float totalDistance;

}

