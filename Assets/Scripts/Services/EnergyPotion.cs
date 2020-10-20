using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : MonoBehaviour
{
    [SerializeField]
    string name;
    [SerializeField]
    float IncreaseValue;
    [SerializeField]
    PlayerController PL;

    [SerializeField]
    float tmpEnergy;
    private void Awake()
    {
        PL = PlayerController.MyPlayerControl;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (name == "Energy")
            {
                tmpEnergy = 0f;
                if (PL.myEnergy.MyCurrentValue <= PL.myEnergy.MyMaxValue)
                {
                    //AudioController.Playsound("Healtht");
                    StartCoroutine(Increase(IncreaseValue));
                    Destroy(gameObject.GetComponentInChildren<SpriteRenderer>());
                    GameObject.FindGameObjectWithTag("EnergyAbsorber").GetComponent<ParticleSystem>().Play();
                }

            }
            else if (name == "TimePower")
            {
                //untuk time Power UP
            }
        }
    }
    private void OnCollision2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (name == "Energy")
            {
                tmpEnergy = 0f;
                if (PL.myEnergy.MyCurrentValue <= PL.myEnergy.MyMaxValue)
                {
                    //AudioController.Playsound("Healtht");
                    StartCoroutine(Increase(IncreaseValue));
                    Destroy(gameObject.GetComponentInChildren<SpriteRenderer>());
                    GameObject.FindGameObjectWithTag("EnergyAbsorber").GetComponent<ParticleSystem>().Play();
                }

                else if (name == "TimePower")
                {
                    //untuk time Power UP
                }
            }
        }
    }
    public IEnumerator Increase(float Val)
    {
        //tmpEnergy = PL.myEnergy.MyCurrentValue;
        //float nextEnergy = PL.myEnergy.MyCurrentValue + Val;
        PL.getEnergy(Val);
        //while (tmpEnergy <= nextEnergy)
        //{
        //    PL.myEnergy.MyCurrentValue++;
        //    tmpEnergy++;
        //    yield return null;
        //}
        Destroy(gameObject);
        yield break;
    }
}
