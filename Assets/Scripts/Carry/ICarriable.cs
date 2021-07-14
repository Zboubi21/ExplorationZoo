using UnityEngine;

public interface ICarriable
{
    Transform GetTransform();
    void PickedUp();
    void PutedDown();
}