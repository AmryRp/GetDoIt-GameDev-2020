using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : Player, ISinkable, IDrainable<float>, IMoveable<float>, IStopable<float>
{

    [SerializeField]
    private Rigidbody CanoeBody;
    public PlayerController(string name, int point, int movespeed) : base(name, point, movespeed)
    {

    }

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
    float delay;
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
    //Cache
    Vector3 PlayerDirection;
    private static PlayerController instance;
    public static PlayerController MyPlayerControl
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
    protected override void Start()
    {
        lastMove = transform.position;
        CanoeBody = GetComponent<Rigidbody>();
        TapCount = 0;

        DontDestroyOnLoad(transform.gameObject);
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetDefault();
        base.Start();
    }

    public void SetDefault()
    {

        Energy.Initialize(PlayerEnergy, PlayerEnergy);
        //MyExp.Initialize(0, Mathf.Floor(100 * myLevel * Mathf.Pow(myLevel, 0.5f)));
        //LevelText.text = myLevel.ToString();

    }
    protected override void Update()
    {
      
        MyScene = SceneManager.GetActiveScene().buildIndex;
        //untuk handle animasi
        //HandleLayers();
        //untuk mobile touch
        TouchScreenMovement();

        //efek arus air
        if (!isMoving)
        {
            StartCoroutine(WatrStreamDirection(0.2f));
        }
        DistanceTravel();
        //untuk drain energy
        Drain();
        base.Update();
        //Untuk Test DiUnity
#if UNITY_EDITOR

        UnityEditorMovement();
#endif
    }

    private void TouchScreenMovement()
    {
        /*direction = MoveDirection();
        //targetPos.Translate(transform.right * WaterStream);
        CanoeBody.MovePosition(transform.position + direction * Time.deltaTime);*/
        TouchScreen();
    }

    private void TouchScreen()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            is_hold = true;
            one_click = true;
            // print("Got touch began!" + Input.touchCount);
            //jika terlalu lama hold / stop
            if (one_click && Hold_timer >= max_Hold)
            {

                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    //print("One Hold Time Reached");
                    StartCoroutine(MovePlayer(0.1f));
                    WaterStreamFlow(DefaultWaterStream);
                    is_hold = false;
                    one_click = false;
                }
            }
            //jika belum melebihi batas waktu hold
            else if (one_click && Hold_timer <= max_Hold - 0.01f)
            {

                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    //print("One Hold Time Safe");
                    StartCoroutine(MovePlayer(MoveSpeed));
                    is_hold = false;
                    one_click = false;
                }
            }
            Hold_timer = 0f;
        }
        //hold
        if (Input.touchCount > 0)
        {
            acumTime += Input.GetTouch(0).deltaTime;

            if (acumTime >= holdTime)
            {
                //Long tap
                //print("Hold");
                StartCoroutine(BreakPlayer(CurrentWaterStream));
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                is_hold = false;
                WaterStreamFlow(DefaultWaterStream);
                acumTime = 0;
            }
        }
        //Double tap
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                TapCount += 1;
            }

            if (TapCount == 1)
            {
                StartCoroutine(MovePlayer(0.8f));
                NewTime = Time.time + MaxDubbleTapTime;
            }
            else if (TapCount == 2 && Time.time <= NewTime)
            {
                //Whatever you want after a dubble tap    
                //print("Dubble tap");
                TapCount = 0;
            }

        }
        if (Time.time > NewTime)
        {
            TapCount = 0;
        }
    }
    private Vector3 WaterFlow(float waterFlow)
    {
        return PlayerDirection = (!InReverse) ?
            PlayerDirection = new Vector3(0f + waterFlow, transform.position.y, transform.position.z) :
            PlayerDirection = new Vector3(transform.position.x + waterFlow, transform.position.y, transform.position.z);
    }

    private Vector3 MoveDirection(float PlayerSpeed)
    {
        return PlayerDirection = (!InReverse) ?
            PlayerDirection = new Vector3(0f + CurrentWaterStream + (PlayerSpeed / 5), transform.position.y, transform.position.z) :
            PlayerDirection = new Vector3(transform.position.x + CurrentWaterStream * (PlayerSpeed / 5), transform.position.y, transform.position.z);
    }

    private void UnityEditorMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            is_hold = true;
            if (!one_click) // first click no previous clicks
            {
                one_click = true;
                // print("Click");
                timer_for_double_click = Time.time; // save the current time
                                                    // do one click things;

            }
            else
            {
                //print("Double");
                one_click = false; // found a double click, now reset

            }

        }

        if ((Input.GetMouseButton(0) && Hold_timer >= max_Hold) || Input.GetMouseButtonUp(0))
        {
            is_hold = false;
            WaterStreamFlow(DefaultWaterStream);
        }
        if (is_hold)
        {
            StartCoroutine(BreakPlayer(CurrentWaterStream));

        }
        else
        {
            //jika terlalu lama hold / stop
            if (one_click && Hold_timer >= max_Hold)
            {

                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    //print("One Hold Time Reached");
                    StartCoroutine(MovePlayer(0.1f));
                    WaterStreamFlow(DefaultWaterStream);
                    one_click = false;
                }
            }
            //jika belum melebihi batas waktu hold
            else if (one_click && Hold_timer <= max_Hold - 0.01f)
            {

                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    // print("One Hold Time Safe");
                    StartCoroutine(MovePlayer(MoveSpeed));
                    one_click = false;
                }
            }
            Hold_timer = 0f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MuddyWatter")
        {
            print("Mud");
            InReverse = false;
            WaterStreamFlow(0.001f);
            MoveSpeed = 0.5f;

        }
        else if (collision.gameObject.tag == "ReverseWatter")
        {
            print("Reverse");
            InReverse = true;
            WaterStreamFlow(-0.003f);
            MoveSpeed = 0.6f;
        }
        else if (collision.gameObject.tag == "AfterWaterfall")
        {
            print("WaterfallEffect");
            InReverse = false;
            WaterStreamFlow(0.006f);
            MoveSpeed = 2f;
        }
        else
        {
            print("Ordinary");
            InReverse = false;


        }
    }
    public IEnumerator MovePlayer(float moveSpeed)
    {
        // Vector3 startingPos = transform.position;
        // Vector3 finalPos = transform.position + (transform.right * MoveSpeed);
        float MoveEffect = moveSpeed;
        float elapsedTime = 0;
        while (elapsedTime < MaxAnimTime)
        {
            PlayerDirection = MoveDirection(moveSpeed - MoveEffect);
            CanoeBody.MovePosition(transform.position + PlayerDirection * Time.deltaTime);
            //transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            MoveEffect -= Time.deltaTime;
            isMoving = (elapsedTime >= MaxAnimTime) ? false : true;
            yield return null;
        }

    }
    public IEnumerator DrainIt()
    {
        float elapsedTime = 0;
        while (elapsedTime < 60f)
        {
            Drain();
            yield return null;
        }

    }

    public IEnumerator BreakPlayer(float breakValue)
    {
        Hold_timer += Time.deltaTime;
        StopCoroutine("MovePlayer");
        if (breakValue > 0)
        {
            CurrentWaterStream -= Mathf.Lerp(0, breakValue, 2f * Time.deltaTime);
            yield return null;
        }
        else
        {
            CurrentWaterStream = 0f;
            yield return null;
        }
    }
    public void WaterStreamFlow(float flow)
    {
        CurrentWaterStream = flow;
    }
    //The required method of the IKillable interface
    public void Kill()
    {
        oncharacterDie();
    }

    //The required method of the IDamageable interface
    public void Damage(float damageTaken)
    {
        
    }

    public IEnumerator WatrStreamDirection(float WaterFlowDir)
    {
        while (!isMoving)
        {
            PlayerDirection = WaterFlow(WaterFlowDir);
            CanoeBody.MovePosition(transform.position + PlayerDirection * Time.deltaTime);
            //transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / moveSpeed));
            yield return null;
        }
    }

    public void Drain()
    {
        if(Hidup)
        {
           
                Fatigue();
            
        }
    }

    public void DistanceTravel()
    {
        
        float distance = Vector3.Distance(lastMove, transform.position);
        totalDistance += distance;
        lastMove = transform.position;
        DistanceTraveled.text = Mathf.Round(totalDistance) + " m";
    }
}
