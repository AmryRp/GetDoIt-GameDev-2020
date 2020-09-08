using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerModel : CharacterModel
{
    [Header("Player Mechanic")]
    [SerializeField]
    protected int PlayerEnergy;
    [SerializeField]
    protected string CanoeType;
    [SerializeField]
    protected float CameraMeter;
    protected static bool playerExists;
    [Header("Player Exp Mechanic")]
    [SerializeField]
    protected Status ExpStat;
    protected Text LevelText;
    public Vector3 lastMove;
    [Header("Player Game Over Canvas UI")]
    [SerializeField]
    public CanvasGroup CGLose;
    protected int scene;

    [Header("Player Energy System")]
    [SerializeField]
    protected float EnergyDrain = 0f;
    protected float batas = 4f;

    private static PlayerController instance;
    public static PlayerController MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
            }
            return instance;
        }
    }

    public PlayerModel(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }

    public Transform MyTarget { get; set; }
    public Animator IsAnimator { get; set; }

    private Vector2 direction;

    protected Rigidbody2D myRigidbody;
    public bool Hidup
    {
        get
        {

            return Energy.MyCurrentValue > 0;
        }
        set
        {
            value = Hidup;
        }

    }

    public bool IsAttacking { get; set; }

    protected bool Skillclick = false;

    [SerializeField]
    private int Level;
    [SerializeField]
    protected Status Energy;
    public Status myEnergy
    {
        get { return Energy; }
    }
    [SerializeField]
    protected float initEnergy;


    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }

    }

    public Vector2 Direction
    {
        get
        {
            return direction;
        }

        set
        {
            direction = value;
        }
    }

    public float Speed
    {
        get
        {
            return MoveSpeed;
        }

        set
        {
            MoveSpeed = value;
        }
    }

    public int myLevel
    {
        get
        {
            return Level;
        }
        set
        {
            Level = value;
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {

        myRigidbody = GetComponent<Rigidbody2D>();
        IsAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }
    public void HandleLayers()
    {
        if (Hidup)
        {
            if (IsMoving)
            {
                ActivateLayer("WalkLayer");
                IsAnimator.SetFloat("x", Direction.x);
                IsAnimator.SetFloat("y", Direction.y);

            }
            else if (IsAttacking)
            {
                ActivateLayer("SerangLayer");
            }
            else if (Skillclick)
            {
                ActivateLayer("Skill");
            }
            else if (!Hidup)
            {

                ActivateLayer("DieLayer");
            }
            else
            {
                ActivateLayer("IdleLayer");
            }
        }

    }

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < IsAnimator.layerCount; i++)
        {
            IsAnimator.SetLayerWeight(i, 0);
        }
        IsAnimator.SetLayerWeight(IsAnimator.GetLayerIndex(layerName), 1);
    }


    public virtual void TakeDamage(float damage, Transform source)
    {

        Energy.MyCurrentValue -= damage;

        GameTextManager.MyInts.creattext(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);

        if (Energy.MyCurrentValue <= 0)
        {
            IsAnimator.SetBool("Die", true);
            Direction = Vector2.zero;
            myRigidbody.velocity = Direction;
            Hidup = false;
            if (this is Player)
            {
                Player.MyInstance.Lose(Player.MyInstance.CGLose);
            }

        }


    }
    public void gethealth(int h)
    {
        myEnergy.MyCurrentValue += h;
        GameTextManager.MyInts.creattext(transform.position, h.ToString(), SCTTYPE.HEAL, true);

    }
    public void Regenerate()
    {

        myEnergy.MyCurrentValue += 5;
        GameTextManager.MyInts.creattext(transform.position, "+", SCTTYPE.HEAL, true);

    }
}

