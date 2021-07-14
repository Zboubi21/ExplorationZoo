using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Vector3 m_InteractionOffset = Vector3.zero;
    [SerializeField] private float m_Radius = 1;
    [SerializeField] private LayerMask m_LayerMask = default;
    [SerializeField] private InteractableType m_InteractionType = default;

    private IInteractable m_CurrentInteractable;
    private IInteractable m_LastInteractable;

    public void CheckInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(GetSpherePosition(), m_Radius, m_LayerMask);

        IInteractable interactable = null;
        for (int i = 0, l = hitColliders.Length; i < l; i++)
        {
            IInteractable tempInteractable = hitColliders[i].GetComponent<IInteractable>();
            if (tempInteractable.GetInteractableType() == m_InteractionType && tempInteractable.CanBeDetected(this))
                interactable = tempInteractable;
        }

        if (interactable != null)
        {
            m_CurrentInteractable = interactable;

            if (m_CurrentInteractable != m_LastInteractable)
            {
                m_LastInteractable?.OnExit(this);
                m_LastInteractable = null;

                m_CurrentInteractable.OnEnter(this);
            }

            m_LastInteractable = m_CurrentInteractable;
        }
        else
        {
            if (m_CurrentInteractable != null)
                m_CurrentInteractable = null;

            if (m_LastInteractable != null)
            {
                m_LastInteractable?.OnExit(this);
                m_LastInteractable = null;
            }
        }
    }

    public bool TryTriggerInteraction()
    {
        if (m_CurrentInteractable != null)
        {
            m_CurrentInteractable.OnInteract(this);
            m_CurrentInteractable = null;
            m_LastInteractable = null;
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetSpherePosition(), m_Radius);
    }

    private Vector3 GetSpherePosition()
    {
        Vector3 position = transform.position;
        position += transform.right * m_InteractionOffset.x;
        position += transform.up * m_InteractionOffset.y;
        position += transform.forward * m_InteractionOffset.z;
        return position;
    }
}