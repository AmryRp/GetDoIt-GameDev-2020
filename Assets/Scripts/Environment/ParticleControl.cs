using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ParticleControl : MonoBehaviour
{
    [Header("Particle Control")]
    [SerializeField]
    string Obstacle;
    [SerializeField]
    protected ParticleSystem WaterSplash;
    [SerializeField]
    protected PlayerController player;
    public void Start()
    {
        WaterSplash = GameObject.FindGameObjectWithTag("PlayerParticle").GetComponent<ParticleSystem>();
    }

    void Awake()
    {
        // Cache references to all desired variables
        player = PlayerController.MyPlayerControl;
        Obstacle = gameObject.tag;
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch (Obstacle)
            {
                case "MuddyWater":
                    player.MoveSpeedInWater = 0.1f;
                    break;
                case "ReverseWater":
                    player.MoveSpeedInWater = -0.2f;
                    break;
                case "AfterWaterfall":
                    player.MoveSpeedInWater = 3f;
                    break;
                case "WaterDrop":
                    player.MoveSpeedInWater = 2f;
                    break;
                case "Stone":
                    player.TakeDamage(20f);
                    
                    break;
                case "Energy":
                    player.getEnergy(15f);

                    break;
                case "Water Volume":
                    if (player.CanoeBody.velocity.y < -4)
                    {
                        AudioController.Playsound("DropSound");
                        WaterSplash.gameObject.SetActive(true);
                        WaterSplash.Play();
                    }
                    break;
                default:
                    player.MoveSpeedInWater = 1f;
                    break;
            }
            BuoyancyEffector2D effectWater;
            effectWater = GameObject.FindGameObjectWithTag("Water").GetComponent<BuoyancyEffector2D>();
            //Debug.Log(effectWater.flowMagnitude + " crash with " + Obstacle);
            effectWater.flowMagnitude = player.MoveSpeedInWater;
        }
    }
}
