using UnityEngine;
using System.Collections;
using System;
public class GeneralEnvironment : MonoBehaviour,  IComparable<GeneralEnvironment>
{
    [SerializeField]
    protected string name;

    [SerializeField]
    public int power;

    public GeneralEnvironment(string newName, int newPower)
    {
        name = newName;
        power = newPower;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(GeneralEnvironment other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return power - other.power;
    }
}
