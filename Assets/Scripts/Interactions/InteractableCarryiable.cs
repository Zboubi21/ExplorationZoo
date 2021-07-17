using System;
using UnityEngine;

public class InteractableCarryiable : MonoBehaviour, IInteractable, ICarryiable
{
    /*********
    * Events *
    *********/
    public event Action OnPickedUp;
    public event Action OnPutedDown;

    /*****************
    * SerializeField *
    *****************/
    [SerializeField] private Material m_DefaultMaterial = null;
    [SerializeField] private Material m_InteractableMaterial = null;
    [SerializeField] private Renderer m_Renderer = null;
    [SerializeField] private Transform m_CarryTransform = null;

    /**********
    * Private *
    **********/
    private InteractableType m_InteractableType = InteractableType.Carryiable;
    private bool m_IsCarryied = false;

    /***************
    * Interactable *
    ***************/
    public virtual InteractableType GetInteractableType() => m_InteractableType;

    public virtual bool CanBeDetected(Interactor _) => !m_IsCarryied;

    public virtual void OnEnter(Interactor _)
    {
        m_Renderer.sharedMaterial = m_InteractableMaterial;
    }

    public virtual void OnInteract(Interactor interactor)
    {
        m_Renderer.sharedMaterial = m_DefaultMaterial;
        interactor.GetComponentInParent<ICarryier>().PickUp(this);
    }

    public virtual void OnExit(Interactor _)
    {
        m_Renderer.sharedMaterial = m_DefaultMaterial;
    }

    /*************
    * Carryiable *
    *************/
    public virtual Transform GetTransform() => m_CarryTransform;

    public virtual void PickedUp()
    {
        m_IsCarryied = true;
        OnPickedUp?.Invoke();
    }

    public virtual void PutedDown()
    {
        m_IsCarryied = false;
        OnPutedDown?.Invoke();
    }
}