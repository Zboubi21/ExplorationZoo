using UnityEngine;

public class Character : MonoBehaviour, ICarrier
{
    [Header("Interactions")]
    [SerializeField] private Interactor m_CarriableInteractor = null;
    [SerializeField] private Interactor m_PutableInteractor = null;
    [Space]
    [SerializeField] private Transform m_PickUpPosition = null;
    [SerializeField] private Transform m_PutDownPosition = null;

    private ICarriable m_CurrentCarriable = null;

    private void Update()
    {
        if (CarryObject())
            m_PutableInteractor.CheckInteractable();
        else
            m_CarriableInteractor.CheckInteractable();
    }

    public void TryTriggerInteraction()
    {
        if (CarryObject())
            m_PutableInteractor.TryTriggerInteraction();
        else
            m_CarriableInteractor.TryTriggerInteraction();
    }

    public bool CarryObject() => m_CurrentCarriable != null;

    public void PickUp(ICarriable carriable)
    {
        Transform carriedTransform = carriable.GetTransform();
        carriedTransform.position = m_PickUpPosition.position;
        carriedTransform.parent = m_PickUpPosition.transform;
        carriable.PickedUp();

        m_CurrentCarriable = carriable;
    }

    public void PutDown()
    {
        Transform carriedTransform = m_CurrentCarriable.GetTransform();
        carriedTransform.position = m_PutDownPosition.position;
        carriedTransform.parent = null;
        m_CurrentCarriable.PutedDown();

        m_CurrentCarriable = null;
    }
}