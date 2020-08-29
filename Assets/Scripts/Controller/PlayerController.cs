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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
