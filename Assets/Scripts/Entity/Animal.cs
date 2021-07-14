using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    /*****************
    * SerializeField *
    *****************/
    [Header("Parameters")]
    [SerializeField] private float m_MoveRadius = 2f;
    [SerializeField] private float m_MinWaitTimeToMove = 2f;
    [SerializeField] private float m_MaxWaitTimeToMove = 4f;
    [SerializeField] private bool m_ReturnToOldMoveAreaWhenMoved = false;

    [Header("References")]
    [SerializeField] private InteractableCarriable m_InteractableCarriable = null;

    /**********
    * Private *
    **********/
    private NavMeshAgent m_Agent;
    private Vector3 m_ReferencePosition;
    private Coroutine m_CheckArea;

    /*****************
    * Initialization *
    *****************/
    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_ReferencePosition = transform.position;

        AddListeners();

        CheckArea();
    }

    private void AddListeners()
    {
        m_InteractableCarriable.OnPickedUp += InteractableCarriable_OnPickedUp;
        m_InteractableCarriable.OnPutedDown += InteractableCarriable_OnPutedDown;
    }

    /************
    * Movements *
    ************/
    private void CheckArea()
    {
        m_CheckArea = StartCoroutine(_CheckArea());
    }

    private void StopCheckArea()
    {
        if (m_CheckArea != null)
            StopCoroutine(m_CheckArea);
    }

    private IEnumerator _CheckArea()
    {
        Vector2 randomPos = Random.insideUnitCircle * m_MoveRadius;
        Vector3 targetPos = m_ReferencePosition + new Vector3(randomPos.x, 0, randomPos.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 0.3f, NavMesh.AllAreas))
            m_Agent.SetDestination(hit.position);

        yield return new WaitForSeconds(Random.Range(m_MinWaitTimeToMove, m_MaxWaitTimeToMove));

        CheckArea();
    }

    /************
    * Carriable *
    ************/
    private void InteractableCarriable_OnPickedUp()
    {
        m_Agent.enabled = false;
        StopCheckArea();
    }

    private void InteractableCarriable_OnPutedDown()
    {
        if (!m_ReturnToOldMoveAreaWhenMoved)
            m_ReferencePosition = transform.position;
        m_Agent.enabled = true;
        CheckArea();
    }

    /*********
    * Gizmos *
    *********/
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(m_ReferencePosition == Vector3.zero ? transform.position : m_ReferencePosition, m_MoveRadius);
    }
}