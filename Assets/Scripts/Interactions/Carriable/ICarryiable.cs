using UnityEngine;

public interface ICarryiable
{
    Transform GetTransform();
    void PickedUp();
    void PutedDown();
}