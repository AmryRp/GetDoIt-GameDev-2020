using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;


//game over when energy zero
public interface ISinkable
{
    void Kill();
    void Drain();
}

//get hit by obstacle
public interface IDrainable<T>
{
    void Damage(T damageTaken);
}

//change equip
public interface IChangeable<T>
{
    void Change(T Type);
}

public interface IMoveable<T>
{
    IEnumerator MovePlayer(T moveSpeed);

    void DistanceTravel();
}
public interface IStopable<T>
{
    IEnumerator BreakPlayer(T breakValue);
}
[System.Serializable]
public class SerializableDictionary<TK, TV>: ISerializable
{
    private Dictionary<TK, TV> _Dictionary;
    [SerializeField] List<TK> _Keys;
    [SerializeField] List<TV> _Values;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new System.NotImplementedException();
    }


}