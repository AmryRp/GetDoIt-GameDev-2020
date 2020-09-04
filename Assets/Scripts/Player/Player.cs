using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PlayerModel, IChangeable<string>
{
    [Header("Player Mechanic")]
    [SerializeField]
    protected int PlayerEnergy;
    [SerializeField]
    protected string CanoeType;
    [SerializeField]
    protected float CameraMeter;

    public Player(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }

    public void Change(string Type)
    {
        Type = CanoeType;
    }
}
