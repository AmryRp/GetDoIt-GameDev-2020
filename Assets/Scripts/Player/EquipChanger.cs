using UnityEngine;
using System.Collections;

public class EquipChanger : MonoBehaviour
{

    [SerializeField]
    protected string EquipName;
    
    //for special speed addition
    [SerializeField]
    protected string EquipAttribute1;
    
    //for special effect
    [SerializeField]
    protected bool EquipAttribute2;

    public EquipChanger(string equipName, string equipAttribute1, bool equipAttribute2)
    {
        EquipName = equipName;
        EquipAttribute1 = equipAttribute1;
        EquipAttribute2 = equipAttribute2;
    }
}
