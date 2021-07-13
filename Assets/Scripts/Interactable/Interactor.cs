using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Vector3 m_InteractionOffset = Vector3.zero;
    [SerializeField] private float m_Radius = 1;
    [SerializeField] private LayerMask m_LayerMask = default;

    private IInteractable m_CurrentInteractable;
    private IInteractable m_LastInteractable;

    private void Update() => CheckInteractable();

    public void CheckInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(GetSpherePosition(), m_Radius, m_LayerMask);

        if (hitColliders.Length > 0)
        {
            m_CurrentInteractable = hitColliders[0].GetComponent<IInteractable>();

            if (m_CurrentInteractable != m_LastInteractable)
            {
                m_LastInteractable?.OnExit();
                m_LastInteractable = null;

                m_CurrentInteractable.OnEnter();
            }

            m_LastInteractable = m_CurrentInteractable;
        }
        else
        {
            m_LastInteractable?.OnExit();
            m_LastInteractable = null;
        }
    }

    public void TriggerInteraction() => m_CurrentInteractable?.OnInteract();

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