using UnityEngine;

public class Character : MonoBehaviour, ICarryier
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
    [SerializeField] private Interactor m_CarryiableInteractor = null;
    [SerializeField] private Interactor m_PutableInteractor = null;
    [SerializeField] private Interactor m_TriggerableInteractor = null;
    [Space]
    [SerializeField] private Transform m_PickUpPosition = null;
    [SerializeField] private Transform m_PutDownPosition = null;
    [Space]
    [SerializeField] private Animator m_Animator = null;

    /**********
    * Private *
    **********/
    private ICarryiable m_CurrentCarryiable = null;

    /**************
    * Interaction *
    **************/
    private void Update()
    {
        if (CarryObject())
            m_PutableInteractor.CheckInteractable();
        else
            m_CarryiableInteractor.CheckInteractable();
        
        m_TriggerableInteractor.CheckInteractable();
    }
    
    public void TryTriggerInteraction()
    {
        if (CarryObject())
            m_PutableInteractor.TryTriggerInteraction();
        else
            m_CarryiableInteractor.TryTriggerInteraction();

        m_TriggerableInteractor.TryTriggerInteraction();
    }

    public bool CarryObject() => m_CurrentCarryiable != null;

    public void PickUp(ICarryiable carryiable)
    {
        Transform carryiedTransform = carryiable.GetTransform();
        carryiedTransform.position = m_PickUpPosition.position;
        carryiedTransform.parent = m_PickUpPosition.transform;
        carryiable.PickedUp();

        m_CurrentCarryiable = carryiable;

        m_Animator.SetLayerWeight(ANIMATOR_CARRY_LAYER_ID, 1);
    }

    public void PutDown()
    {
        Transform carryiedTransform = m_CurrentCarryiable.GetTransform();
        carryiedTransform.position = m_PutDownPosition.position;
        carryiedTransform.parent = null;
        m_CurrentCarryiable.PutedDown();

        m_CurrentCarryiable = null;

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