using UnityEngine;

using System.Collections;
public delegate void HealthChanged(float energy);
public delegate void CharacterDie();
public class Player : PlayerModel, IChangeable<string>
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

    public void Change(string Type)
    {
        Type = CanoeType;
    }

  
    //public Status MyExp { get { return ExpStat; } set => ExpStat = value; }


    // Use this for initialization
    protected virtual void Start()
    {

        //myRigidbody = GetComponent<Rigidbody2D>();
        IsAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
    public void Lose()
    {
        Time.timeScale = 1f;
        GameObject.Find("CanvasLose").GetComponent<Canvas>().enabled = true;
        StartCoroutine(UI.CalculatingPrefabPoint());
    }
    //public void GainExp(int exp)
    //{
    //    MyExp.MyCurrentValue += exp;

    //    if (MyExp.MyCurrentValue >= MyExp.MyMaxValue)
    //    {
    //        StartCoroutine(Blip());
    //    }
    //}
    //private IEnumerator Blip()
    //{
    //    while (!MyExp.ExpFull)
    //    {
    //        yield return null;
    //    }
    //    myLevel++;
    //    LevelText.text = myLevel.ToString();
    //    MyExp.MyMaxValue = 100 * myLevel * Mathf.Pow(myLevel, 0.5f);
    //    MyExp.MyMaxValue = Mathf.Floor(MyExp.MyMaxValue);
    //    MyExp.MyCurrentValue = MyExp.MyOverExp;
    //    MyExp.Reset();
    //}
    //public void UpdateLevel()
    //{
    //    LevelText.text = myLevel.ToString();
    //}
    public void PlayerRespawn()
    {
        Hidup = true;
        //IsAnimator.SetBool("IsDie", false);
        Energy.MyCurrentValue += myEnergy.MyMaxValue;
    }
    //untuk handle animasi
    //public void HandleLayers()
    //{
    //    if (Hidup)
    //    {
    //        if (IsMoving)
    //        {
    //            ActivateLayer("WalkLayer");
    //            IsAnimator.SetFloat("x", Direction.x);
    //            IsAnimator.SetFloat("y", Direction.y);

    //        }
    //        else if (IsOnMove)
    //        {
    //            ActivateLayer("SerangLayer");
    //        }
    //        else if (CaptureClick)
    //        {
    //            ActivateLayer("Skill");
    //        }
    //        else if (!Hidup)
    //        {

    //            ActivateLayer("DieLayer");
    //        }
    //        else
    //        {
    //            ActivateLayer("IdleLayer");
    //        }
    //    }

    //}

    //public void ActivateLayer(string layerName)
    //{
    //    for (int i = 0; i < IsAnimator.layerCount; i++)
    //    {
    //        IsAnimator.SetLayerWeight(i, 0);
    //    }
    //    IsAnimator.SetLayerWeight(IsAnimator.GetLayerIndex(layerName), 1);
    //}


    public virtual void TakeDamage(float damage/*, Transform source*/)
    {

        Energy.MyCurrentValue -= damage;
        AudioController.Playsound("HitObstacle");
        //GameTextManager.MyInts.creattext(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false);

        if (Energy.MyCurrentValue <= 0)
        {
            //IsAnimator.SetBool("IsDie", true);
            Direction = Vector2.zero;
            //myRigidbody.velocity = Direction;
            Hidup = false;
            if (this is Player && !Hidup)
            {
                Lose();
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
        myEnergy.MyCurrentValue -= EnergyDrain ;
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