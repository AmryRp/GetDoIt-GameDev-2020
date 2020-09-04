using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [Header("Model Attribute")]
    [SerializeField]
    public string CharacterName;

    [SerializeField]
    protected int Point;

    [SerializeField]
    protected float MoveSpeed;

    public CharacterModel(string name, int point)
    {
        CharacterName = name;
        Point = point;
    }
    public CharacterModel(string name, int point,int movespeed)
    {
        CharacterName = name;
        Point = point;
        MoveSpeed = movespeed;
    }
}
