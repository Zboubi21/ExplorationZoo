using UnityEngine;

public class InteractableGround : MonoBehaviour, IInteractable
{
    private InteractableType m_InteractableType = InteractableType.Putable;

    public InteractableType GetInteractableType() => m_InteractableType;

    public bool CanBeDetected(Interactor _) => true;

    public void OnEnter(Interactor _)
    {
        //Debug.Log("InteractableGround_OnEnter", this);
    }

    public void OnInteract(Interactor interactor)
    {
        //Debug.Log("InteractableGround_OnInteract", this);
        interactor.GetComponentInParent<ICarryier>().PutDown();
    }

    public void OnExit(Interactor _)
    {
        //Debug.Log("InteractableGround_OnExit", this);
    }
}