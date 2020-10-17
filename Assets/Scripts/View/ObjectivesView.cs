using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesView : MonoBehaviour
{
    private static ObjectivesView instance;
    public static ObjectivesView MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectivesView>();
            }
            return instance;
        }
    }
    public int objectiveId;
    public Image Icons;
    public Text objectiveName;
    public Image checkAcomplished;
    public Text Description;
    public Text objectiveStatus;

}
