using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ObjectivesService : IUpdateable<float>
{
    public int objectiveId;
    public string objectiveName;
    public string objectiveDesc;
    public float currentValue,targetValue;
    public bool isAcomplished = false;
    public bool AcomplishObjective(float value)
    {
        
        currentValue = value;
        if (currentValue >= targetValue)
        {
            isAcomplished = true;
        }
        else 
        {
            isAcomplished = false;
        }
        return (isAcomplished);
    }

}
