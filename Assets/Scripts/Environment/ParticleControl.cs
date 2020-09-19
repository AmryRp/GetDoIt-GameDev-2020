using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        player = FindObjectOfType<PlayerController>();
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
                case "Water Volume":
                    if (player.CanoeBody.velocity.y < -2)
                    {
                        WaterSplash.gameObject.SetActive(true);
                        WaterSplash.Play();
                        print(player.CanoeBody.velocity);
                    }
                    break;
                default:
                    player.MoveSpeedInWater = 0.5f;
                    break;
            }
            Debug.Log("crash");
        }
    }
}
