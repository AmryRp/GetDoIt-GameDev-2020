using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ObjectivesService 
{
   
    public int objectiveId;
    public string objectiveName;
    public string objectiveDesc;
    public float currentValue,targetValue;
    public bool isAcomplished = false;
    [TextArea(6, 10)]
    public string Description;
   
}
