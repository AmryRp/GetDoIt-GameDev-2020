using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerModel : CharacterModel
{
    protected const string IDLE = "IsIdle";
    protected const string MOVING = "IsMoving";
    protected const string STOP = "IsStop";
    protected const string CAPTURE = "IsCapture";

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
/*[SerializeField]
    protected float LerpSpeed;
    public Transform targetPos;
    //private Vector3 targetPos;*/
    public float DefaultWaterStream;
    public bool InReverse;
    public float MoveSpeedInWater;
    public bool isMoving = false;
    public float MaxAnimTime;

    //Cache

    [Header("Player Mechanic")]
    [SerializeField]
    protected float PlayerEnergy;
    [SerializeField]
    protected string CanoeType;
    [SerializeField]
    protected float CameraMeter;
    protected static bool playerExists;
    [SerializeField]
    public Rigidbody2D CanoeBody;

    [Header("Player Moving Animation Particle")]
    [SerializeField]
    protected ParticleSystem MoveBoatSplash;
    //[SerializeField]
    //protected Status ExpStat;
    //protected Text LevelText;
    public Vector3 lastMove;
    protected int scene;

    [Header("Player Energy System")]
    [SerializeField]
    protected float EnergyDrain = 0f;
    protected float batas = 4f;
    public float maxDrainSpeedTime = 100f;
    public float timeWaitingDrain =1.2f;
    public GameManager GM;

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
    public float totalDistance;
    public float AllDistance;
    [Header("Player Point Records System")]
    [SerializeField]
    public UIManager UI;
    public float AllShotTaken;
    public float AllSavedDistance;
    public float AllPointCollected;
    public float AllObjectShot;
    public float AllAnimalShot;
}

