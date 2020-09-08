using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SCTTYPE { DAMAGE, HEAL }
public class GameTextManager : MonoBehaviour
{
    private static GameTextManager instance;

    public static GameTextManager MyInts
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameTextManager>();
            }
            return instance;
        }
    }
    [SerializeField]
    private GameObject combattextpf;


    public void creattext(Vector2 pos, string text, SCTTYPE type, bool crit)
    {
        pos.y += 0.09f;
        Text combat = Instantiate(combattextpf, transform).GetComponent<Text>();
        combat.transform.position = pos;

        string operation = string.Empty;

        switch (type)
        {
            case SCTTYPE.DAMAGE:
                operation += "-";
                combat.color = Color.white;
                break;
            case SCTTYPE.HEAL:
                operation += "+";
                combat.color = Color.green;
                break;
        }
        combat.text = operation + text;
        if (crit)
        {
            combat.GetComponent<Animator>().SetBool("Crit", crit);
        }
    }
}