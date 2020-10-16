using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjectivesPrefab;
    [SerializeField]
    private List<ObjectivesService> objectiveServices;
    public bool Objective1, Objective2,Objctive3;
    public ObjectivesView myObjectiveRandom { get; set; }

    private static ObjectivesManager instance;
    public static ObjectivesManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectivesManager>();
            }
            return instance;
        }
    }
}
