using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Animal : MonoBehaviour
{
    /***********
    * Constant *
    ***********/
    private const string ANIMATION_MOVE_NAME = "Move";
    private const string ANIMATION_Carry_NAME = "Carry";

    /*****************
    * SerializeField *
    *****************/
    [Header("Parameters")]
    [SerializeField] private float m_MoveRadius = 2f;
    [SerializeField] private float m_MinWaitTimeToMove = 2f;
    [SerializeField] private float m_MaxWaitTimeToMove = 4f;
    [SerializeField] private bool m_ReturnToOldMoveAreaWhenMoved = false;

    [Header("References")]
    [SerializeField] private InteractableCarryiable m_InteractableCarryiable = null;
    [SerializeField] private Animator m_Animator = null;

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
        m_InteractableCarryiable.OnPickedUp += InteractableCarryiable_OnPickedUp;
        m_InteractableCarryiable.OnPutedDown += InteractableCarryiable_OnPutedDown;
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

    /*************
    * Carryiable *
    *************/
    private void InteractableCarryiable_OnPickedUp()
    {
        m_Agent.enabled = false;
        StopCheckArea();
        m_Animator.SetBool(ANIMATION_Carry_NAME, true);
    }

    private void InteractableCarryiable_OnPutedDown()
    {
        if (!m_ReturnToOldMoveAreaWhenMoved)
            m_ReferencePosition = transform.position;
        m_Agent.enabled = true;
        CheckArea();
        m_Animator.SetBool(ANIMATION_Carry_NAME, false);
    }

    private void Update()
    {
        m_Animator.SetFloat(ANIMATION_MOVE_NAME, m_Agent.velocity.magnitude.Remap(0, m_Agent.speed, 0, 1));
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