public interface IInteractable
{
    InteractableType GetInteractableType();
    bool CanBeDetected(Interactor interactor);
    void OnEnter(Interactor interactor);
    void OnInteract(Interactor interactor);
    void OnExit(Interactor interactor);
}