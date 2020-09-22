using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{   
    public GameObject[] tiles;
    private Transform player;
    [SerializeField]
    private float SpawnTileAt = 0.0f;
    [SerializeField]
    private float tilelength = 31.7f;
    [SerializeField]
    private float save = 10f;
    [SerializeField]
    private int amntilescreen = 1;
    [SerializeField]
    private int lastPrefabindex;
    [SerializeField]
    float Water = 0;
    [SerializeField]
    private List<GameObject> activeTiles;

    // Start is called before the first frame update
    private void Start()
    {
        activeTiles = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < amntilescreen; i++)
        {
            if (i < 2)
                spawntile(1);
            else
                spawntile(0);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (player.position.x - save > (SpawnTileAt - amntilescreen * tilelength))
        {
            spawntile(0);

        }
        if (activeTiles.Count > 4)
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
        Water += Time.deltaTime;
        if (tiles.Length <= 1)
            return 0;
        int randomIndx = lastPrefabindex;

        if (Water >= 0.6f)
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
            Water = 0;
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
        float rand = UnityEngine.Random.Range(1, 1.5f);
        Vector3 Ini = new Vector3(transform.position.x + SpawnTileAt, player.transform.position.y - rand, transform.position.z);
        go.transform.position = Ini;
        SpawnTileAt += tilelength;
        activeTiles.Add(go);


    }

}
