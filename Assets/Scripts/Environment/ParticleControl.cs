using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ParticleControl : MonoBehaviour
{
    [Header("Particle COntrol")]
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
    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            switch (Obstacle)
            {
                case "MuddyWatter":
                    player.MoveSpeedInWater = 0.5f;
                    break;
                case "ReverseWatter":
                    player.MoveSpeedInWater = -0.6f;
                    break;
                case "AfterWaterfall":
                    player.MoveSpeedInWater = 2f;
                    break;
                case "WaterDrop":
                    player.MoveSpeedInWater = 2f;
                    break;
                case "Stone":
                    player.TakeDamage(20f);
                    AudioController.Playsound("HitObstacle");
                    break;
                case "Energy":
                    player.getEnergy(15f);

                    break;
                case "Water Volume":
                    if (player.CanoeBody.velocity.y < -1)
                    {
                        AudioController.Playsound("DropSound");
                        WaterSplash.gameObject.SetActive(true);
                        WaterSplash.Play();
                        //print(player.CanoeBody.velocity);
                    }
                    break;
                default:
                    player.MoveSpeedInWater = 0.5f;
                    break;
            }
            BuoyancyEffector2D effectWater;
            effectWater = GameObject.FindGameObjectWithTag("Water").GetComponent<BuoyancyEffector2D>();
            effectWater.flowMagnitude = player.MoveSpeedInWater;
            //Debug.Log("crash");
        }
    }
}
