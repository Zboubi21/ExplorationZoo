using GameDevStack.Physics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : Controller
{
    /*****************
    * SerializeField *
    *****************/
    [Header("Movements")]
    [SerializeField] private float m_MaxMovementSpeed = 7.5f;
    [SerializeField] private float m_AccelerationMovementSpeed = 2.5f;
    [SerializeField] private float m_DecelerationMovementSpeed = 2.5f;

    [Header("Rotations")]
    [SerializeField] private float m_MaxRotationSpeed = 1.5f;
    [SerializeField] private float m_AccelerationRotationSpeed = 0.25f;
    [SerializeField] private float m_DecelerationRotationSpeed = 0.25f;
    [Space]
    [SerializeField] private bool m_UseFollowerForward = true;
    [SerializeField] private Transform m_Follower = null;
    [SerializeField] private Floater[] m_Floaters = null;
    [SerializeField] private Interactor m_PutableInteractor = null;

    /**********
    * Private *
    **********/
    private Rigidbody m_Rigidbody;
    private Vector3 m_InputDirectionNormalized;
    private float m_CurrentMovementSpeed;
    private float m_CurrentMovementVelocity;
    private float m_CurrentRotationSpeed;
    private float m_CurrentRotationVelocity;

    /*****************
    * Initialization *
    *****************/
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    /***********
    * Controls *
    ***********/
    public override void UpdateMoveInput(Vector2 inputDirection, bool hasInput)
    {
        base.UpdateMoveInput(inputDirection, hasInput);
        m_InputDirectionNormalized = m_InputDirection.normalized;
    }

    public override void StopControl()
    {
        base.StopControl();
        m_InputDirectionNormalized = Vector3.zero;
    }

    /*********
    * Update *
    *********/
    private void Update()
    {
        UpdateSpeeds();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
        Float();
    }

    /*******
    * Core *
    *******/
    private void UpdateSpeeds()
    {
        float targetRotationSpeed = Mathf.Lerp(0, m_MaxRotationSpeed, GetMaxAbsInput(false));
        float targetMovementSpeed = Mathf.Lerp(0, m_MaxMovementSpeed, GetMaxAbsInput(false));

        float accelerationDecelerationRotationSpeed = m_CurrentRotationSpeed < targetRotationSpeed ? m_AccelerationRotationSpeed : m_DecelerationRotationSpeed;
        float accelerationDecelerationMovementSpeed = m_CurrentMovementSpeed < targetMovementSpeed ? m_AccelerationMovementSpeed : m_DecelerationMovementSpeed;

        m_CurrentRotationSpeed = Mathf.SmoothDamp(m_CurrentRotationSpeed, targetRotationSpeed, ref m_CurrentRotationVelocity, accelerationDecelerationRotationSpeed);
        m_CurrentMovementSpeed = Mathf.SmoothDamp(m_CurrentMovementSpeed, targetMovementSpeed, ref m_CurrentMovementVelocity, accelerationDecelerationMovementSpeed);
    }

    private void Rotate()
    {
        Vector3 relativePos = m_HasInput ? m_InputDirectionNormalized : m_LastInputDirection;
        m_LastInputDirection = relativePos;

        if (relativePos != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(relativePos, Vector3.up);
            Quaternion currentRotation = Quaternion.Slerp(transform.rotation, targetRotation, m_CurrentRotationSpeed * Time.deltaTime);
            m_Rigidbody.MoveRotation(currentRotation);
        }
    }

    private void Move()
    {
        if (m_UseFollowerForward)
        {
            m_Follower.position = transform.position;
            m_Follower.rotation = transform.rotation;
            m_Follower.rotation = Quaternion.Euler(0, m_Follower.eulerAngles.y, 0);
        }

        Vector3 forward = m_UseFollowerForward ? m_Follower.forward : transform.forward;
        m_Rigidbody.MovePosition(m_Rigidbody.position + forward * m_CurrentMovementSpeed * Time.deltaTime);
    }

    private void Float()
    {
        for (int i = 0, l = m_Floaters.Length; i < l; i++)
            m_Floaters[i].CustomUpdate();
    }

    /*********
    * Helper *
    *********/
    private float GetMaxAbsInput(bool normalized)
    {
        Vector3 inputs = normalized ? m_InputDirectionNormalized : m_InputDirection;
        float xInput = Mathf.Abs(inputs.x);
        float zInput = Mathf.Abs(inputs.z);
        return xInput > zInput ? xInput : zInput;
    }
}