using UnityEngine;

public class InteractableTriggerable : MonoBehaviour, IInteractable
{
    /*****************
    * SerializeField *
    *****************/
    [SerializeField] private Renderer m_Renderer = null;
    [SerializeField] private Material m_DefaultMaterial = null;
    [SerializeField] private Material m_InteractableMaterial = null;

    /**********
    * Private *
    **********/
    private InteractableType m_InteractableType = InteractableType.Triggerable;

    /***************
    * Interactable *
    ***************/
    public bool CanBeDetected(Interactor _)
    {
        return true;
    }

    public InteractableType GetInteractableType() => m_InteractableType;

    public Transform GetTransform() => transform;

    public void OnEnter(Interactor _)
    {
        m_Renderer.sharedMaterial = m_InteractableMaterial;
    }

    public void OnInteract(Interactor interactor)
    {
        m_Renderer.sharedMaterial = m_DefaultMaterial;
        ICarryiable carryiable = interactor.GetComponentInParent<ICarryiable>();
        carryiable.PickedUp();
        GetComponentInParent<ICarryier>().PickUp(carryiable);
    }

    public void OnExit(Interactor _)
    {
        m_Renderer.sharedMaterial = m_DefaultMaterial;
    }
}