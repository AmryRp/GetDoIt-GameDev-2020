﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlayerController : Player, ISinkable, IDrainable<float>, IMoveable<float>, IStopable<float>
{


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
        lastMove = transform.position;
        CanoeBody = GetComponent<Rigidbody2D>();
        TapCount = 0;
        DistanceTraveled = GameObject.FindGameObjectWithTag("T_Distance").GetComponent<Text>();
        Energy = GameObject.FindGameObjectWithTag("bar energy").GetComponent<Status>();
        Energy.Initialize(PlayerEnergy, PlayerEnergy);
        //MyExp.Initialize(0, Mathf.Floor(100 * myLevel * Mathf.Pow(myLevel, 0.5f)));
        //LevelText.text = myLevel.ToString();

    }
    protected override void Update()
    {
        //MyScene = SceneManager.GetActiveScene().buildIndex;
        //untuk handle animasi
        //HandleLayers();
        //untuk mobile touch 
        //print(!GameManager.MyGM.IsPaused);
        if (!GameManager.MyGM.IsPaused)
        {
            TouchScreen();
            DistanceTravel();
            StartCoroutine(DrainIt());
        }
        //untuk drain energy
        //Drain();
        base.Update();
        //Untuk Test DiUnity
#if UNITY_EDITOR

        //UnityEditorMovement();
#endif
    }

    private void TouchScreen()
    {
        if (!IsMoving)
        {
            MoveBoatSplash.Stop();
        }
        if ( Input.touchCount > 0 && 
            !(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {

                one_click = true;
                timer_for_double_click = Time.time;
                //Debug.Log("Not Touched the UI");
                if (!is_hold)
                {
                    StartCoroutine(MovePlayer(MoveSpeed));
                    one_click = false;
                }

            }
            //hold
            if (Input.touchCount > 0 )
            {

                acumTime += Input.GetTouch(0).deltaTime;

                if (acumTime >= holdTime)
                {
                    //Long tap
                    //print("Hold");
                    is_hold = true;
                    StartCoroutine(BreakPlayer(MoveSpeedInWater));

                    if ((acumTime >= max_Hold) && (Time.time - timer_for_double_click) > delay)
                    {
                        print("One Hold Time Reached");
                        StartCoroutine(MovePlayer(0.6f));
                        is_hold = false;
                        one_click = false;
                        acumTime = 0f;
                    }
                }
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                is_hold = false;
                acumTime = 0;
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
                    NewTime = Time.time + MaxDubbleTapTime;
                    //jika terlalu lama hold / stop
                    if (one_click)
                    {
                        // if the time now is delay seconds more than when the first click started. 
                        if ((Time.time - timer_for_double_click) > delay)
                        {
                            print("One Hold Time Safe");
                            StartCoroutine(MovePlayer(MoveSpeed));

                            is_hold = false;
                            one_click = false;
                        }
                    }
                    Hold_timer = 0f;

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
    }

    private void UnityEditorMovement()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
           
                if (!one_click) // first click no previous clicks
            {
                one_click = true;
                timer_for_double_click = Time.time;
                is_hold = true;
            }
            else
            {
                //print("Double");
                one_click = false; // found a double click, now reset
            }

        }
        if (Input.GetMouseButtonUp(0) || Hold_timer >= max_Hold)
        {
            is_hold = false;
            Hold_timer = 0f;
        }
        if (is_hold)
        {
            StartCoroutine(BreakPlayer(MoveSpeedInWater));
        }
        else
        {
            is_hold = false;
            //jika terlalu lama hold / stop
            if (one_click && Hold_timer >= max_Hold)
            {

                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    print("One Hold Time Reached");
                    StartCoroutine(MovePlayer(0.5f));
                    one_click = false;
                }
            }
            //jika belum melebihi batas waktu hold
            else if (one_click && Hold_timer <= max_Hold - 0.01f)
            {

                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    print("One Hold Time Safe");
                    StartCoroutine(MovePlayer(MoveSpeed));

                    MoveBoatSplash.Play();
                    one_click = false;
                }
            }
            Hold_timer = 0f;
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
            /*Vector2 tempVect = new Vector2(waterSpeed, 0f);
            tempVect = tempVect.normalized * moveSpeed * Time.deltaTime;*/

            //Untuk pergerakan

            MoveBoatSplash.Play();
            CanoeBody.AddForce(new Vector2(((MoveSpeedInWater + moveSpeed) * Time.deltaTime), 0), ForceMode2D.Impulse); // Movement
            //MovementSpeedInWater kecepatan canoe berdasarkan deras air atau bisa ditambah dengan moveSpeed kecepatan dari player, seperti dibawah ini
            /*CanoeBody.MovePosition(transform.position + transform.right * ((MoveSpeedInWater + moveSpeed) * Time.deltaTime)); */

            //transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            MoveEffect -= Time.deltaTime;
            isMoving = (elapsedTime >= MaxAnimTime) ? false : true;
            
            yield return null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water")) // Ketika canoe bersentuhan dengan water
        {
            BuoyancyEffector2D effectWater;
            effectWater = collision.gameObject.GetComponent<BuoyancyEffector2D>();
            effectWater.flowMagnitude = DefaultWaterStream;
            MoveSpeedInWater = effectWater.flowMagnitude; //Set Canoe Water speed berdasarkan deras aliran air yang disentuh
            Debug.Log("Touch with water");
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
        if (breakValue > 0 && is_hold)
        {
            print("Stop");
            float elapsedTime = 0;
            while (elapsedTime < max_Hold)
            {
                //Untuk berhenti
                CanoeBody.velocity = Vector2.zero; // Movement
                elapsedTime += Time.deltaTime;
                if (CanoeBody.velocity == Vector2.zero)
                {
                    MoveBoatSplash.Stop();
                }
                //isMoving = (elapsedTime >= MaxAnimTime) ? true : false;
                breakValue -= Time.deltaTime;
                yield return null;
            }
        }
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

    public void Drain()
    {
        if (Hidup)
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
    //public bool speedLimiter(float Vel)
    //{
    //    bool LimitReach;
    //    CanoeBody
    //    return LimitReach = ;
    //}
}
