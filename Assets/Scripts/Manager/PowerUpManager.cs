using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject[] coins;
    public float CoinTime;
    public float HealthTime = 30f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnCoins());
        StartCoroutine(SpawnHealths());
    }
    IEnumerator SpawnCoins()
    {
        yield return new WaitForSeconds(CoinTime);
        Spawn();
    }
    IEnumerator SpawnHealths()
    {
        yield return new WaitForSeconds(HealthTime);
        Spawn2();
    }
    void Spawn()
    {
        int RandCoind = UnityEngine.Random.Range(0, 1);
        Vector3 hpos = new Vector3(player.position.x + 10f, 0f, 0f);
        Instantiate(coins[RandCoind], hpos, coins[RandCoind].transform.rotation);

        StartCoroutine(SpawnCoins());
    }
    void Spawn2()
    {
        int RandCoind = UnityEngine.Random.Range(1, 2);


        Vector3 hpos = new Vector3(player.position.x + 30f, 0f, 0f);
        Instantiate(coins[RandCoind], hpos, coins[RandCoind].transform.rotation);

        StartCoroutine(SpawnHealths());
    }
    // Update is called once per frame
}
