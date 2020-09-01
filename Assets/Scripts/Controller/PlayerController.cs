using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    private float holdTime = 0.8f; //or whatever
    private float acumTime = 0;
    int TapCount;
    public float MaxDubbleTapTime;
    float NewTime;
    private float waterStream = 1f;
    void Start()
    {
        TapCount = 0;
    }

    void Update()
    {
        Vector3 movePl = new Vector3(1f, 0f, 0f);
        transform.position += movePl * waterStream * Time.deltaTime;
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
    }
    public void MovePlayer()
    {
        Vector3 movePl = new Vector3(1f, 0f, 0f);
        movePl += movePl * MoveSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, transform.position + movePl, MoveSpeed * Time.deltaTime);
    }
}

