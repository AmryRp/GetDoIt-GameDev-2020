using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
public class PlayerController : Player, ISinkable, IDrainable<float>, IMoveable<float>, IStopable<float>
{

    [SerializeField]
    private Rigidbody CanoeBody;
    public PlayerController(string name, int point, int movespeed) : base(name, point, movespeed)
    {

    }

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
        

        SetDefault();
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
        base.Start();
    }

    public void SetDefault()
    {
        DistanceTraveled = GameObject.FindGameObjectWithTag("T_Distance").GetComponent<Text>();
        Energy = GameObject.FindGameObjectWithTag("bar energy").GetComponent<Status>();
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
        
        
            StartCoroutine(DrainIt());
        
        //untuk drain energy
        //Drain();
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
            PlayerDirection = new Vector3(0f + waterFlow + CurrentWaterStream, transform.position.y, transform.position.z) :
            PlayerDirection = new Vector3(transform.position.x + waterFlow + CurrentWaterStream, transform.position.y, transform.position.z);
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

    void OnTriggerEnter(Collider collision)
    {
        WaterSplash = GetComponentInChildren<ParticleSystem>();
        if (collision.gameObject.tag == "MuddyWatter")
        {
            print("Mud");
            InReverse = false;
            WaterStreamFlow(0.03f);
            MoveSpeed = 0.5f;

        }
        else if (collision.gameObject.tag == "ReverseWatter")
        {
            print("Reverse");
            InReverse = true;
            WaterStreamFlow(-0.3f);
            MoveSpeed = 0.6f;
        }
        else if (collision.gameObject.tag == "AfterWaterfall")
        {
            print("WaterfallEffect");
            InReverse = false;
            WaterStreamFlow(2.2f);
            MoveSpeed = 2f;
        }
        else if (collision.gameObject.tag == "WaterDrop")
        {
            print("WaterDrop");
            InReverse = false;
            WaterStreamFlow(1.5f);
            MoveSpeed = 2f;
        }
        else if (collision.gameObject.tag == "Water Volume" && CanoeBody.velocity.y < -5)
        { 
            print("Water SPLASH");
            WaterSplash.gameObject.SetActive(true);
            WaterSplash.Play();
        }
        else
        {
            print("Ordinary");
            WaterStreamFlow(DefaultWaterStream);
            InReverse = false;
            print(CanoeBody.velocity);
            WaterSplash.Stop();
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
        while (elapsedTime < 100f)
        {
            Drain();
            elapsedTime += Time.deltaTime;
            yield return new WaitForSecondsRealtime(1.3f);
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
