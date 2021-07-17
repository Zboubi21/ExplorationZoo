using UnityEngine;

public class Character : MonoBehaviour, ICarrier
{
    /***********
    * Constant *
    ***********/
    private const string ANIMATION_MOVE_NAME = "Move";
    private const int ANIMATOR_CARRY_LAYER_ID = 1;

    /*****************
    * SerializeField *
    *****************/
    [Header("Interactions")]
    [SerializeField] private Interactor m_CarriableInteractor = null;
    [SerializeField] private Interactor m_PutableInteractor = null;
    [Space]
    [SerializeField] private Transform m_PickUpPosition = null;
    [SerializeField] private Transform m_PutDownPosition = null;
    [Space]
    [SerializeField] private Animator m_Animator = null;

    /**********
    * Private *
    **********/
    private ICarriable m_CurrentCarriable = null;

    /*********
    * Update *
    *********/
    private void Update()
    {
        if (CarryObject())
            m_PutableInteractor.CheckInteractable();
        else
            m_CarriableInteractor.CheckInteractable();
    }

    /**************
    * Interaction *
    **************/
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

        m_Animator.SetLayerWeight(ANIMATOR_CARRY_LAYER_ID, 1);
    }

    public void PutDown()
    {
        Transform carriedTransform = m_CurrentCarriable.GetTransform();
        carriedTransform.position = m_PutDownPosition.position;
        carriedTransform.parent = null;
        m_CurrentCarriable.PutedDown();

        m_CurrentCarriable = null;

        m_Animator.SetLayerWeight(ANIMATOR_CARRY_LAYER_ID, 0);
    }

    /************
    * Animation *
    ************/
    public void SetMoveAnimation(float moveValue)
    {
        m_Animator.SetFloat(ANIMATION_MOVE_NAME, moveValue);
    }
}