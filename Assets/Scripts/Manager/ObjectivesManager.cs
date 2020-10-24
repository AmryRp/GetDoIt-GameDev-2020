using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesManager : MonoBehaviour
{
    public string[] Animal = { "rusa", "elang", "any" };
    public string[] ObjectiveTitle = { "Distance Travelled ", "Photo Takens ", "Animal " };
    public int[] distanceTarget = { 300, 450, 650, 1000 };
    public int[] photoTakenTarget = { 12, 24, 36, 52 };
    public int[] animalCatch = { 10, 20, 50, 100, 80 };
    [SerializeField]
    private GameObject ObjectivesPrefab;
    [SerializeField]
    private List<ObjectivesService> objectiveServices;
    public bool[] Objective;
    public GameObject[] ObjMultipliericon;
    public ObjectivesView myObjectiveRandom { get; set; }
    public ObjectivesView[] objectsLoaded;
    public string animalName = "animal";
    public Sprite[] ObjectiveIcons;
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

    public void LoadObjectives()
    {
        PlayerController PCtr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        objectsLoaded = new ObjectivesView[objectiveServices.Count];
        // this data to load from json to UI view
        for (int i = 0; i < objectiveServices.Count; i++)
        {
            //add new ItemShop
            myObjectiveRandom = Instantiate(ObjectivesPrefab, ObjectivesManager.MyInstance.transform).GetComponent<ObjectivesView>();
            //add Init Name,Price,ItemPreview
            myObjectiveRandom.objectiveId = i;
            string Desc = "Complete the challenge ";
            float counterMax = 500; 
            if (i != 2)
            {
                myObjectiveRandom.objectiveName.text = ObjectiveTitle[i] + " Reached : ";
                if (i == 0)
                {
                    if (PCtr.playerTimeMode)
                    {
                        if (!PlayerPrefs.GetFloat("DistanceChekPoint").Equals(0))
                        {
                            counterMax = Mathf.Round(PlayerPrefs.GetFloat("DistanceChekPoint"));
                        }
                        objectiveServices[i].targetValue = counterMax;
                        myObjectiveRandom.objectiveName.text += counterMax;
                        Desc += " " + ObjectiveTitle[i] + " reach " + counterMax + " meters";
                        objectsLoaded[i] = myObjectiveRandom;
                    }
                    else 
                    {
                        counterMax = ObjectiveType(distanceTarget, i);
                        myObjectiveRandom.objectiveName.text += counterMax;
                        Desc += " " + ObjectiveTitle[i] + " reach " + counterMax + " meters";
                        objectsLoaded[i] = myObjectiveRandom;
                    }
                }
                else
                {
                    counterMax = ObjectiveType(photoTakenTarget, i);
                    myObjectiveRandom.objectiveName.text += counterMax;
                    Desc += " " + ObjectiveTitle[i] + " reach " + counterMax + " shots";
                    objectsLoaded[i] = myObjectiveRandom;
                }
            }
            else
            {
                objectsLoaded[i] = myObjectiveRandom;
                int random = UnityEngine.Random.Range(0, Animal.Length);
                float name = ObjectiveType(animalCatch, i);
                string animalNametemp = Animal[random];
                myObjectiveRandom.objectiveName.text = ObjectiveTitle[i] + animalNametemp + " Captured Reached " + name;
                animalName = animalNametemp;
                Desc += " " + ObjectiveTitle[i] + " reach " + name + " shots with specified animal '" +
                    animalName + "'";
            }

            myObjectiveRandom.Description.text = Desc;
            myObjectiveRandom.Icons.sprite = ObjectiveIcons[i];
            myObjectiveRandom.objectiveStatus.text = objectiveServices[i].currentValue + " / " + objectiveServices[i].targetValue;
        }
    }
    public float ObjectiveType(int[] list, int id)
    {
        bool acomplished = false;
        if (PlayerPrefs.HasKey("Acomplished" + id))
        {
            acomplished = PlayerPrefs.GetInt("Acomplished" + id) == 1 ? true : false;
        }
        if (acomplished)
        {
            int rand = Random.Range(0, list.Length);
            objectiveServices[id].targetValue = list[rand];
        }
        else
        {
            objectiveServices[id].targetValue = list[0];
        }

        return (objectiveServices[id].targetValue);
    }
    public bool updateObjective(int ItemID, float val)
    {
        bool result = false;


        if (objectsLoaded[ItemID].objectiveId == objectiveServices[ItemID].objectiveId)
        {
            float current = objectiveServices[ItemID].currentValue;
            float target = objectiveServices[ItemID].targetValue;
            objectsLoaded[ItemID].objectiveStatus.text = Mathf.Round(current).ToString() + " / " + Mathf.Round(target).ToString();
        }

        objectiveServices[ItemID].currentValue = val;
        if (objectiveServices[ItemID].currentValue >= objectiveServices[ItemID].targetValue)
        {
            Objective[ItemID] = true;
            objectiveServices[ItemID].isAcomplished = true;
            objectsLoaded[ItemID].checkAcomplished.GetComponent<Image>().enabled = true;
            PlayerPrefs.SetInt("Acomplished" + ItemID, Objective[ItemID] ? 1 : 0);
            PlayerPrefs.Save();
            result = false;
            return result;
           
        }
        else
        {
            Objective[ItemID] = false;
            objectiveServices[ItemID].isAcomplished = false;
            objectsLoaded[ItemID].checkAcomplished.GetComponent<Image>().enabled = false;
            result = true;
        }



        return result;
    }
    public bool AcomplishObjective(float value, string name, int id)
    {
        bool isAcomplished = false;
        if (id == 0)
        {
            isAcomplished = ObjectivesManager.MyInstance.updateObjective(id, value);
        }
        else if (id == 1)
        {
            isAcomplished = ObjectivesManager.MyInstance.updateObjective(id, value);
        }
        else if (id == 2)
        {
            isAcomplished = ObjectivesManager.MyInstance.updateObjective(id, value);
        }
        else
        {
            //nothing
        }
        return (isAcomplished);
    }

    public void updateMultiplier()
    {

    }
}
