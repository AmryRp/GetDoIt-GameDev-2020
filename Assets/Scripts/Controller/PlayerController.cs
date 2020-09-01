using System.Collections;
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

    // Update is called once per frame
    private float holdTime = 0.8f; //or whatever
    private float acumTime = 0;
    int TapCount;
    public float MaxDubbleTapTime;
    float NewTime;
    private float WaterStream = 5f;
    void Start()
    {
        TapCount = 0;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {

            print("Got touch began!");
        }
        targetPos = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, LerpSpeed * Time.deltaTime);
        //hold
        //if (Input.touchCount > 0)
        //{
        //    acumTime += Input.GetTouch(0).deltaTime;

        //    if (acumTime >= holdTime)
        //    {
        //        //Long tap
        //        print("Hold");
        //    }

        //    if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        acumTime = 0;
        //    }
        //}

        ////Double tap
        //if (Input.touchCount == 1)
        //{
        //    Touch touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Ended)
        //    {
        //        TapCount += 1;
        //    }

        //    if (TapCount == 1)
        //    {

        //        NewTime = Time.time + MaxDubbleTapTime;
        //    }
        //    else if (TapCount == 2 && Time.time <= NewTime)
        //    {

        //        //Whatever you want after a dubble tap    
        //        print("Dubble tap");


        //        TapCount = 0;
        //    }

        //}
        //if (Time.time > NewTime)
        //{
        //    TapCount = 0;
        //}
    }
    [SerializeField]
    protected float LerpSpeed;

    public Vector3 targetPos;
    public void MovePlayer()
    {
        targetPos = new Vector3(transform.position.x + MoveSpeed, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, LerpSpeed * Time.deltaTime);
    }
    private void OnMouseDown()
    {
        MovePlayer();
    }
}
