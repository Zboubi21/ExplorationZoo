using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private const string INPUT_MOVE = "Move";
    private const string INPUT_INTERACT = "Interact";

    [SerializeField] private float m_MoveSpeed = 5f;

    private PlayerInput m_PlayerInput;
    private InputAction m_MoveAction;
    private InputAction m_InteractAction;

    private void Awake()
    {
        m_PlayerInput = GetComponent<PlayerInput>();

        m_MoveAction = m_PlayerInput.currentActionMap.FindAction(INPUT_MOVE);
        m_InteractAction = m_PlayerInput.currentActionMap.FindAction(INPUT_INTERACT);

        m_InteractAction.started += InteractAction_Started;
    }

    private void InteractAction_Started(InputAction.CallbackContext context)
    {
        //Debug.Log("Interaction started");
    }

    private void Update()
    {
        Vector2 input = m_MoveAction.ReadValue<Vector2>();
        Vector3 moveInput = new Vector3(input.x, 0, input.y);
        //Debug.Log("Look = " + moveInput);

        if (moveInput.sqrMagnitude >= 0.01f)
        {
            Vector3 newPosition = transform.position + moveInput * Time.deltaTime * m_MoveSpeed;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newPosition, out hit, 0.3f, NavMesh.AllAreas))
            {
                transform.position = hit.position;
            }
        }
    }
}