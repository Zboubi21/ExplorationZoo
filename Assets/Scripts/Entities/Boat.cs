using UnityEngine;
using UnityEngine.AI;

public class Boat : MonoBehaviour, ICarryier
{
    [SerializeField] private Interactor m_PutableInteractor = null;
    [SerializeField] private Transform m_CharacterPosition = null;

    /**********
    * Private *
    **********/
    private ICarryiable m_CurrentCarryiable = null;

    public void TryTriggerInteraction()
    {
        m_PutableInteractor.TryTriggerInteraction();
    }

    /*********
    * Update *
    *********/
    private void Update()
    {
        m_PutableInteractor.CheckInteractable();
    }

    /***********
    * Carryier *
    ***********/
    public bool CarryObject() => m_CurrentCarryiable != null;

    public void PickUp(ICarryiable carryiable)
    {
        PlayerManager.Instance.SwitchController(ControlType.Boat);

        Transform carryiableTransform = carryiable.GetTransform();
        carryiableTransform.position = m_CharacterPosition.position;
        carryiableTransform.rotation = m_CharacterPosition.rotation;
        carryiableTransform.parent = m_CharacterPosition;

        m_CurrentCarryiable = carryiable;
    }

    public void PutDown()
    {
        Transform carryiedTransform = m_CurrentCarryiable.GetTransform();
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, Mathf.Infinity, NavMesh.AllAreas))
            carryiedTransform.position = hit.position;
        carryiedTransform.parent = null;

        m_CurrentCarryiable.PutedDown();
        m_CurrentCarryiable = null;

        PlayerManager.Instance.SwitchController(ControlType.Character);
    }
}