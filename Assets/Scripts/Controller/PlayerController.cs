using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Player, ISinkable, IDrainable<float>, IMoveable<float>, IStopable<float>
{
    [SerializeField]
    private Rigidbody2D CanoeBody;
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
    [Header("Water Effect")]
    [SerializeField]
    protected float LerpSpeed;
    public Transform targetPos;
    //private Vector3 targetPos;
    public float WaterStream;

    void Start()
    {

        CanoeBody = GetComponent<Rigidbody2D>();
        TapCount = 0;
    }

    void Update()
    {
        targetPos.Translate(transform.right * WaterStream);
        transform.position = Vector3.MoveTowards(transform.position, targetPos.position.normalized, WaterStream * Time.deltaTime);
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            print("Got touch began!");
        }
        //hold
        if (Input.touchCount > 0)
        {
            acumTime += Input.GetTouch(0).deltaTime;

            if (acumTime >= holdTime)
            {
                //Long tap
                print("Hold");
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
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
                print("Dubble tap");
                TapCount = 0;
            }

        }
        if (Time.time > NewTime)
        {
            TapCount = 0;
        }

        //Untuk Test DiUnity
#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
                is_hold = true;
                if (!one_click) // first click no previous clicks
                {
                    one_click = true;
                    print("Click");
                    timer_for_double_click = Time.time; // save the current time
                                                        // do one click things;
                }
                else
                {
                    print("Double");
                    one_click = false; // found a double click, now reset

                }
            
        }

        if ((Input.GetMouseButton(0) && Hold_timer >= max_Hold)|| Input.GetMouseButtonUp(0))
        {
            is_hold = false;
        }        
        if (is_hold)
        {
            StartCoroutine(BreakPlayer(WaterStream));
            
        }
        else
        {
            //jika terlalu lama hold / stop
            if (one_click && Hold_timer >= max_Hold)
            {
                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    print("One Hold Time Reached");
                    StartCoroutine(MovePlayer(2f));
                    WaterStreamFlow(0.003f);
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
                    StartCoroutine(MovePlayer(0.8f));
                    WaterStreamFlow(0.003f);
                    one_click = false;
                }
            }
            Hold_timer = 0f;
        }
#endif
    }
    public IEnumerator MovePlayer(float moveSpeed)
    {
        Vector3 startingPos = transform.position;
        Vector3 finalPos = transform.position + (transform.right * MoveSpeed);
        float elapsedTime = 0;
        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(startingPos, finalPos, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator BreakPlayer(float breakValue)
    {
        Hold_timer += Time.deltaTime;
        if (breakValue > 0)
        {
            WaterStream -= Mathf.Lerp(0, breakValue, 2f * Time.deltaTime);
            yield return null;
        }
        else
        {
            WaterStream = 0f;
            yield return null;
        }
        //if (Hold_timer >= max_Hold)
        //{
        //    is_hold = false;
        //}
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MuddyWatter")
        {
            WaterStreamFlow(0.001f);
            MoveSpeed = 0.5f;
        }
        else if (collision.gameObject.tag == "ReverseWatter")
        {
            WaterStreamFlow(-0.003f);
            MoveSpeed = 0.6f;
        }
        else if (collision.gameObject.tag == "AfterWaterfall")
        {
            WaterStreamFlow(0.006f);
            MoveSpeed = 2f;
        }
        else
        {
            WaterStreamFlow(0.003f);
        }
    }
    public void WaterStreamFlow(float flow)
    {
        WaterStream = flow;
    }
    //The required method of the IKillable interface
    public void Kill()
    {
        //Do something fun
    }

    //The required method of the IDamageable interface
    public void Drain(float damageTaken)
    {
        //Do something fun
    }
}
