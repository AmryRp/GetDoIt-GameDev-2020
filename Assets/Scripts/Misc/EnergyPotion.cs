using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPotion : MonoBehaviour
{
    [SerializeField]
    float HealthIncrease;
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
            tmpEnergy = 0f;
            if (PL.myEnergy.MyCurrentValue <= PL.myEnergy.MyMaxValue)
            {
                //AudioController.Playsound("Healtht");
                StartCoroutine(Increase(HealthIncrease));
                Destroy(gameObject.GetComponentInChildren<SpriteRenderer>());
                GameObject.FindGameObjectWithTag("EnergyAbsorber").GetComponent<ParticleSystem>().Play();
            }

        }
    }

    public IEnumerator Increase(float damage)
    {
        tmpEnergy = PL.myEnergy.MyCurrentValue;
        float nextEnergy = PL.myEnergy.MyCurrentValue + damage;
        while (tmpEnergy <= nextEnergy)
        {
            PL.myEnergy.MyCurrentValue++;
            tmpEnergy++;
            yield return null;
        }

    }
}
