using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private const string INPUT_MOVE = "Move";
    private const string INPUT_INTERACT = "Interact";

    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_RotationSpeed = 1.5f;
    [Space]
    [SerializeField] private Character m_Character = null;

    private PlayerInput m_PlayerInput;
    private InputAction m_MoveAction;
    private InputAction m_InteractAction;
    private Vector2 m_MoveInputs;
    private Vector3 m_LastInputDirection;

    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();

        m_MoveAction = m_PlayerInput.currentActionMap.FindAction(INPUT_MOVE);
        m_InteractAction = m_PlayerInput.currentActionMap.FindAction(INPUT_INTERACT);

        m_InteractAction.started += InteractAction_Started;
    }

    private void InteractAction_Started(InputAction.CallbackContext context)
    {
        m_Character.TryTriggerInteraction();
    }

    private void Update()
    {
        m_MoveInputs = m_MoveAction.ReadValue<Vector2>();
        Vector3 moveInput = new Vector3(m_MoveInputs.x, 0, m_MoveInputs.y);
        bool hasInput = HasInput();

        Move(moveInput, hasInput);
        Rotate(moveInput, hasInput);
    }

    private void Move(Vector3 moveInput, bool hasInput)
    {
        if (hasInput)
        {
            Vector3 newPosition = transform.position + moveInput * Time.deltaTime * m_MoveSpeed;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPosition, out hit, 0.3f, NavMesh.AllAreas))
                transform.position = hit.position;
        }
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

    private bool HasInput() => m_MoveInputs.sqrMagnitude >= 0.01f;
}