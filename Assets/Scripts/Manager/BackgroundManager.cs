using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] tiles;
    private Transform player;
    private float spawnz = 0.0f;
    private float tilelength = 21.5f;
    private float save = 11f;
    private int amntilescreen = 10;
    private int lastPrefabindex;
    float crossroad = 0;
    private List<GameObject> activeTiles;

    // Start is called before the first frame update
    private void Start()
    {
        activeTiles = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amntilescreen; i++)
        {
            if (i < 2)
                spawntile(0);
            else
                spawntile();
        }
    }

    // Update is called once per frame
    private void Update()
    {

        if (player.position.x - save > (spawnz - amntilescreen * tilelength))
        {
            spawntile();

        }
        if (activeTiles.Count > 15)
        {
            DeleteTile();
        }
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int randPrefab()
    {
        crossroad += Time.deltaTime;
        if (tiles.Length <= 1)
            return 0;
        int randomIndx = lastPrefabindex;

        if (crossroad >= 0.6f)
        {
            while (randomIndx == lastPrefabindex)
            {
                int min = 0;
                if (randomIndx == 1)
                {
                    min = 5;
                }
                else if (randomIndx == 2)
                {
                    min = 6;
                }
                else if (randomIndx == 3)
                {
                    min = 7;
                }
                else if (randomIndx == 4)
                {
                    min = 8;
                }
                else if (randomIndx == 5)
                {
                    min = 9;
                }
                else
                {
                    min = 0;
                }
                randomIndx = Random.Range(min, tiles.Length);
            }
            crossroad = 0;
            lastPrefabindex = randomIndx;
            return randomIndx;
        }
        else
        {
            while (randomIndx == lastPrefabindex)
            {
                randomIndx = Random.Range(0, 5);
            }
            lastPrefabindex = randomIndx;
            return randomIndx;
        }

    }
    private void spawntile(int prefabIndex = -1)
    {


        GameObject go;
        if (prefabIndex == -1)
        {
            go = Instantiate(tiles[randPrefab()]) as GameObject;
        }
        else
        {
            go = Instantiate(tiles[prefabIndex]) as GameObject;
        }
        go.transform.SetParent(transform);
        go.transform.position = Vector3.right * spawnz;
        spawnz += tilelength;
        activeTiles.Add(go);


    }

}
