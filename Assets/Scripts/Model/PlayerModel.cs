using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerModel : CharacterModel
{
    [Header("TouchScreen")]
    public float holdTime = 0.8f; //or whatever
    public float acumTime = 0;
    public int TapCount;
    public float MaxDubbleTapTime;
    public float NewTime;

    [Header("UnityEditor")]
    public bool one_click = false;
    public bool timer_running;
    [SerializeField]
    public float timer_for_double_click;
    //this is how long in seconds to allow for a double click
    [SerializeField]
    public float delay;
    public bool is_hold = false;
    public float Hold_timer;
    public float max_Hold;

    [Header("Water Stream")]
    [SerializeField]
    protected float LerpSpeed;
    public Transform targetPos;
    //private Vector3 targetPos;
    public float CurrentWaterStream;
    public float DefaultWaterStream;
    public bool InReverse;
    public bool isMoving = false;
    public float MaxAnimTime;

    [Header("Water Splash")]
    [SerializeField]
    protected ParticleSystem WaterSplash;

    //Cache
   
    [Header("Player Mechanic")]
    [SerializeField]
    protected float PlayerEnergy;
    [SerializeField]
    protected string CanoeType;
    [SerializeField]
    protected float CameraMeter;
    protected static bool playerExists;
    public Vector3 PlayerDirection;
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

    [Header("Distance Travel System")]
    [SerializeField]
    protected Text DistanceTraveled;
    [SerializeField]
    protected float totalDistance;

}

