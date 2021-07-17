using UnityEngine;

public interface IInteractable
{
    InteractableType GetInteractableType();
    Transform GetTransform();
    bool CanBeDetected(Interactor interactor);
    void OnEnter(Interactor interactor);
    void OnInteract(Interactor interactor);
    void OnExit(Interactor interactor);
}