
using UnityEngine;

using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class GeneralEnvironment : MonoBehaviour, IComparable<GeneralEnvironment>
{
    [SerializeField]
    public string name;
    [SerializeField]
    public int myLevel = 1;
    [SerializeField]
    public int power = 10;
    [SerializeField]
    public bool isCaptured = false;
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
    public float maxRayCastDefault = 40f;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DistanceReceiver") && !isCaptured)
        {
            Ray ray = new Ray(transform.position, other.transform.position);
            float distance = Vector3.Distance(transform.position, other.transform.position);
            RaycastHit hit;
            // Debug.DrawLine(transform.position, other.transform.position,Color.red);
            isCaptured = true;
            // jika tidak terhalangi jarak pandang object ke camera
            if (!Physics.Raycast(ray, out hit, distance))
            {
                // Debug.DrawLine(hit.point,hit.point+Vector3.up*7,Color.white);
                int rand = UnityEngine.Random.Range(0, 9999);
                name = name + "_" + Mathf.Round(distance).ToString() + "_" + power.ToString() + "_" + rand.ToString();
                //print("Found Object : " + name);
                Dictionary<string, float> obx = CameraObjectManager.MyCamReceiver.ObjectCatchs;
                int MaxObx = CameraObjectManager.MyCamReceiver.MaxObjects;
                List<string> LObx = CameraObjectManager.MyCamReceiver.KeyVal;
                if (obx.Count >= MaxObx)
                {
                    obx.Remove(LObx[0]);
                    LObx.RemoveAt(0);
                }
                if (!CameraObjectManager.MyCamReceiver.ObjectCatchs.ContainsKey(name))
                {
                    CameraObjectManager.MyCamReceiver.ObjectCatchs.Add(name, distance);
                    CameraObjectManager.MyCamReceiver.KeyVal.Add(name);
                }
                StartCoroutine(CameraObjectManager.MyCamReceiver.AddingObjects());
            }
        }
        

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DistanceReceiver") && isCaptured)
        {
            CameraObjectManager.MyCamReceiver.ObjectCatchs.Remove(name);
            CameraObjectManager.MyCamReceiver.KeyVal.Remove(name);
            //print("Remove Object : " + name);
            StartCoroutine(CameraObjectManager.MyCamReceiver.AddingObjects());
            name = "object";
            isCaptured = false;
        }
    }
    public void start()
    {
        BoxCollider Box = GetComponent<BoxCollider>();
        Box.isTrigger = true;

    }
}

