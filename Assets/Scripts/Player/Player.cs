using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerModel, IChangeable<string>
{
    [SerializeField]
    protected int PlayerEnergy;
    [SerializeField]
    protected string CanoeType;

    public void Change(string Type)
    {
        Type = CanoeType;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
