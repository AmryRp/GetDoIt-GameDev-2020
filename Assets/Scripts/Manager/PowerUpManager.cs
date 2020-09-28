using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] PowerUp;
    public GameObject[] Animal;
    public float PowerUpSpawnTime;
    public float AnimalSpawnTime = 30f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnPower());
        StartCoroutine(SpawnAnimal());
    }
    IEnumerator SpawnPower()
    {
        yield return new WaitForSeconds(PowerUpSpawnTime);
        Spawn();
    }
    IEnumerator SpawnAnimal()
    {
        yield return new WaitForSeconds(AnimalSpawnTime);
        Spawn2();
    }
    void Spawn()
    {
        int RandCoind = UnityEngine.Random.Range(0, 1);
        Vector3 hpos = new Vector3(player.position.x + 26f, 0f, 0f);
        Instantiate(PowerUp[RandCoind], hpos, PowerUp[RandCoind].transform.rotation);

        StartCoroutine(SpawnPower());
    }
    void Spawn2()
    {
        int RandCoind = UnityEngine.Random.Range(1, 2);
        Vector3 hpos = new Vector3(player.position.x + 45f, 0f, 0f);
        Instantiate(Animal[RandCoind], hpos, Animal[RandCoind].transform.rotation);

        StartCoroutine(SpawnAnimal());
    }
   
}
