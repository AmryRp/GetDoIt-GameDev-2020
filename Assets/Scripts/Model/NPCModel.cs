using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCModel : CharacterModel
{
    public float maxcatchtime;
    public bool flipped;
    public float movedir;

    public NPCModel(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }
}
