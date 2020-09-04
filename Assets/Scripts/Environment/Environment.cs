using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : GeneralEnvironment
{
    public Environment(string newName, int newPower) : base(newName, newPower)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, GeneralEnvironment> Obstacles = new Dictionary<string, GeneralEnvironment>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
