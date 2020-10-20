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
    public int OdadingLimit;
    [SerializeField]
    private List<GameObject> ActivePower;
    [SerializeField]
    private List<GameObject> ActiveAnimal;
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
        GameObject PowerObj;
        int RandCoind = UnityEngine.Random.Range(0, 1);
        Vector3 hpos = new Vector3(player.position.x + 17f, player.position.y+0.2f, 0f);
        PowerObj=Instantiate(PowerUp[RandCoind], hpos, PowerUp[RandCoind].transform.rotation) as GameObject;
        ActivePower.Add(PowerObj);
        StartCoroutine(SpawnPower());
        if (ActivePower.Count > OdadingLimit)
        {
            DeleteTile(ActivePower);
        }
    }

    private void DeleteTile(List<GameObject> Name)
    {
        Destroy(Name[0]);
        Name.RemoveAt(0);
    }
    void Spawn2()
    {
        GameObject PowerObj;
        int RandCoind = UnityEngine.Random.Range(0, Animal.Length);
        Vector3 hpos = new Vector3(player.position.x + 45f, player.position.y + 0.2f, 0f);
        PowerObj = Instantiate(Animal[RandCoind], hpos, Animal[RandCoind].transform.rotation) as GameObject;
        StartCoroutine(SpawnAnimal());
        ActiveAnimal.Add(PowerObj);
        if (ActiveAnimal.Count > OdadingLimit)
        {
            DeleteTile(ActiveAnimal);
        }
    }
   
}
