using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : CharacterModel
{
    private static PlayerModel instance;
    public static PlayerModel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerModel>();
            }
            return instance;
        }
    }
    private static bool playerExists;

    public PlayerModel(string name, int point, int movespeed) : base(name, point, movespeed)
    {
    }


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        if (!playerExists)
        {
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
