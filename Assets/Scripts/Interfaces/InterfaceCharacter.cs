using System.Collections;
using System.Collections.Generic;
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