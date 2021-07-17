using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Controller
{
    /*****************
    * SerializeField *
    *****************/
    [Header("Parameters")]
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_RotationSpeed = 1.5f;
    
    [Header("References")]
    [SerializeField] private Character m_Character = null;

    /***********
    * Controls *
    ***********/
    public override void StartControl()
    {
        base.StartControl();
        gameObject.SetActive(true);
    }

    public override void StopControl()
    {
        base.StopControl();
        gameObject.SetActive(false);
    }

    public override void InteractAction_Started()
    {
        base.InteractAction_Started();
        m_Character.TryTriggerInteraction();
    }

    /*******
    * Core *
    *******/
    private void Update()
    {
        Move(m_InputDirection, m_HasInput);
        Rotate(m_InputDirection, m_HasInput);
    }

    private void Move(Vector3 inputDirection, bool hasInput)
    {
        if (hasInput)
        {
            Vector3 newPosition = transform.position + inputDirection * Time.deltaTime * m_MoveSpeed;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPosition, out hit, 0.3f, NavMesh.AllAreas))
                transform.position = hit.position;
        }
        m_Character.SetMoveAnimation(inputDirection.magnitude);
    }

    private void Rotate(Vector3 moveInput, bool hasInput)
    {
        Vector3 inputDirection = hasInput ? moveInput : m_LastInputDirection;
        m_LastInputDirection = inputDirection;
        if (inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
            Quaternion currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, m_RotationSpeed * Time.deltaTime);
            transform.rotation = currentRotation;
        }
    }
}