using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//game over when energy zero
public interface ISinkable
{
    void Kill();
}

//get hit by obstacle
public interface IDrainable<T>
{
    void Drain(T damageTaken);
}

//change equip
public interface IChangeable<T>
{
    void Change(T Type);
}
