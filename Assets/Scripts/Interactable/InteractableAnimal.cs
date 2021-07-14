using UnityEngine;

public class InteractableAnimal : MonoBehaviour, IInteractable, ICarriable
{
    /*****************
    * SerializeField *
    *****************/
    [SerializeField] private Material m_DefaultMaterial = null;
    [SerializeField] private Material m_InteractableMaterial = null;
    [SerializeField] private Renderer m_Renderer = null;

    /**********
    * Private *
    **********/
    private InteractableType m_InteractableType = InteractableType.Carriable;
    private bool m_IsCarried = false;

    /***************
    * Interactable *
    ***************/
    public InteractableType GetInteractableType() => m_InteractableType;

    public bool CanBeDetected(Interactor _) => !m_IsCarried;

    public void OnEnter(Interactor _)
    {
        //Debug.Log("OnEnter", this);
        m_Renderer.sharedMaterial = m_InteractableMaterial;
    }

    public void OnInteract(Interactor interactor)
    {
        //Debug.Log("OnInteract", this);
        interactor.GetComponentInParent<ICarrier>().PickUp(this);
    }

    public void OnExit(Interactor _)
    {
        //Debug.Log("OnExit", this);
        m_Renderer.sharedMaterial = m_DefaultMaterial;
    }

    /************
    * Carriable *
    ************/
    public Transform GetTransform() => transform;

    public void PickedUp()
    {
        m_IsCarried = true;
    }

    public void PutedDown()
    {
        m_IsCarried = false;
    }
}