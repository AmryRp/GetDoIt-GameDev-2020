using UnityEngine;

using System.Collections;
public delegate void HealthChanged(float energy);
public delegate void CharacterDie();
public class Player : PlayerModel
{
    public event CharacterDie characterdie;
    public event HealthChanged healthchanged;
    public void OnHealthChanged(float energy)
    {
        if (healthchanged != null)
        {
            healthchanged(energy);

        }
    }
    public void oncharacterDie()
    {
        if (characterdie != null)
        {
            characterdie();
        }
        Destroy(gameObject);
    }

    private static Player instance;
    public static Player MyInstPL
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    public Player(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }

    //public Status MyExp { get { return ExpStat; } set => ExpStat = value; }


    // Use this for initialization
    protected virtual void Start()
    {
        float str = PlayerPrefs.GetFloat("MyPoint");
        float str2 = PlayerPrefs.GetFloat("DistanceTraveled");
        int str3 = PlayerPrefs.GetInt("MyShot");
        if (!str.Equals(0f)) AllPointCollected = PlayerPrefs.GetFloat("MyPoint");
        if (!str2.Equals(0f))
        {
            AllDistance = PlayerPrefs.GetFloat("DistanceTraveled");
            AllSavedDistance = AllDistance;
        }
        if (!str3.Equals(0)) AllShotTaken = PlayerPrefs.GetInt("MyShot");

        //myRigidbody = GetComponent<Rigidbody2D>();
        IsAnimator = GetComponentInChildren<Animator>();
        LowHP = GameObject.FindGameObjectWithTag("bar energy").GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    bool lose = false;
    public virtual IEnumerator Lose()
    {
        UI = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<UIManager>();
        Time.timeScale = 1f;

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        StartCoroutine(UI.CalculatingPrefabPoint());
        UI.LoadUI(false, false, false, false, false, false, true, false, false, false);
        Animator LoseAnim = GameObject.FindGameObjectWithTag("GameOver").GetComponent<Animator>();
        LoseAnim.SetBool("PlayerLose", true);
        //UI = UIManager.MyUI;
        yield return null;
        yield break;
    }
    public void PlayerRespawn()
    {
        Hidup = true;
        Energy.MyCurrentValue += myEnergy.MyMaxValue;
    }

    public virtual void TakeDamage(float damage/*, Transform source*/)
    {
        if (!playerTimeMode)
        {
            Energy.MyCurrentValue -= damage;
            //AudioController.Playsound("HitObstacle");
            //GameTextManager.MyInts.creattext(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);

            if (Energy.MyCurrentValue <= 0)
            {
                //IsAnimator.SetBool("IsDie", true);
                Direction = Vector2.zero;
                //myRigidbody.velocity = Direction;
                Hidup = false;
                if (this is Player && !Hidup)
                {
                    print("from player energy");
                    StartCoroutine(Lose());
                }

            }
        }
    }
    public void getEnergy(float h)
    {
        myEnergy.MyCurrentValue += h;
        //text tambah energy
        // GameTextManager.MyInts.creattext(transform.position, h.ToString(), SCTTYPE.HEAL, true);

    }
    public void Fatigue()
    {
        //dikali dengan canoe weight nantinya
        if (!playerTimeMode)
        {
            myEnergy.MyCurrentValue -= EnergyDrain;
        }
        PlayerEnergy = myEnergy.MyCurrentValue;
        //GameTextManager.MyInts.creattext(transform.position, "-", SCTTYPE.HEAL, true);

    }

    public bool Hidup
    {
        get
        {

            return Energy.MyCurrentValue > 0;
        }
        set
        {
            _ = Hidup;
        }

    }
    public Status myEnergy
    {
        get { return Energy; }
        set
        {
            _ = Energy.MyCurrentValue;
        }
    }


    public bool IsMoving
    {
        get
        {
            return Direction.x != 0;
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

}