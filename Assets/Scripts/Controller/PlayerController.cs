﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Player, ISinkable, IDrainable<float>
{
    public PlayerController(string name, int point, int movespeed) : base(name, point, movespeed)
    {

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
    [Header("TouchScreen")]
    // Update is called once per frame
    
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
    [SerializeField]
    //this is how long in seconds to allow for a double click
    float delay;
    [Header("Water Effect")]
    [SerializeField]
    protected float LerpSpeed;
    public Vector3 targetPos;
    public float WaterStream = 0.3f;


    void Start()
    {
        TapCount = 0;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            print("Got touch began!");
        }
        targetPos = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, WaterStream * Time.deltaTime);
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
                MovePlayer();
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


#if UNITY_EDITOR

        if (Input.GetMouseButtonDown(0))
        {
            
            if (!Input.GetMouseButtonUp(0))
            {
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
        }
        if (one_click)
        {
           
            while (one_click)
            {
                holdTime += Time.deltaTime;
            }
            if (!Input.GetMouseButtonUp(0))
            {
                // if the time now is delay seconds more than when the first click started. 
                if ((Time.time - timer_for_double_click) > delay)
                {
                    print("One");
                    //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
                    MovePlayer();
                    one_click = false;

                }
            }
        }
#endif
    }


    public void MovePlayer()
    {
        targetPos = new Vector3(transform.position.x + MoveSpeed, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, LerpSpeed * Time.deltaTime);
    }
    public void BreakPlayer()
    {

    }
}
