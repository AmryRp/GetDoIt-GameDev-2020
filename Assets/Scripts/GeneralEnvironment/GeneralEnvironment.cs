<<<<<<< Updated upstream
﻿using UnityEngine;
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
=======
﻿using UnityEngine;

using System.Collections;
using System;
public class GeneralEnvironment : MonoBehaviour, IComparable<GeneralEnvironment>
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int myLevel;
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
    public float maxRayCastDefault;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DistanceReceiver"))
        {


            Ray ray = new Ray(transform.position, other.transform.position);
            float distance = Vector3.Distance(transform.position, other.transform.position);
            RaycastHit hit;

            int rand = UnityEngine.Random.Range(0, 9999);
            name = name + "_" + Mathf.Round(distance).ToString() + "_" + rand.ToString();
            Debug.DrawLine(transform.position, other.transform.position);

            print("Found Object : " + name);
            CameraObjectManager.MyCamReceiver.ObjectCatchs.Add(name, distance);
            CameraObjectManager.MyCamReceiver.KeyVal.Add(name);
            print(CameraObjectManager.MyCamReceiver.ObjectCatchs.Count);
            StartCoroutine(CameraObjectManager.MyCamReceiver.AddingObjects());
        }
        else { print("nothing"); }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DistanceReceiver"))
        {
           
            CameraObjectManager.MyCamReceiver.ObjectCatchs.Remove(name);
            CameraObjectManager.MyCamReceiver.KeyVal.Remove(name);
            print("Remove Object : " + name);
            StartCoroutine(CameraObjectManager.MyCamReceiver.AddingObjects());

        }
    }
}


>>>>>>> Stashed changes
