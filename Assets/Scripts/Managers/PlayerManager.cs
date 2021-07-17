using UnityEngine;
using GameDevStack.Patterns;
using UnityEngine.InputSystem;

/*******
* Enum *
*******/
public enum ControlType { None, Character, Boat }

[RequireComponent(typeof(PlayerInput))]
public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
    /***********
    * Constant *
    ***********/
    private const string INPUT_MOVE = "Move";
    private const string INPUT_INTERACT = "Interact";

    /*****************
    * SerializeField *
    *****************/
    [HideInInspector] [SerializeField] private ControlType m_ControlType = ControlType.None;
    [SerializeField] private Controller m_PlayerController = null;
    [SerializeField] private Controller m_BoatController = null;

    /**********
    * Getters *
    **********/
    public ControlType ControlType => m_ControlType;

    /**********
    * Private *
    **********/
    private PlayerInput m_PlayerInput;
    private InputAction m_MoveAction;
    private InputAction m_InteractAction;
    private Vector2 m_MoveInput;

    /*****************
    * Initialization *
    *****************/
    protected override void Awake()
    {
        base.Awake();

        m_PlayerInput = GetComponent<PlayerInput>();

        m_MoveAction = m_PlayerInput.currentActionMap.FindAction(INPUT_MOVE);
        m_InteractAction = m_PlayerInput.currentActionMap.FindAction(INPUT_INTERACT);

        m_InteractAction.started += InteractAction_Started;
    }

    /*********
    * Inputs *
    *********/
    private void InteractAction_Started(InputAction.CallbackContext context)
    {
        switch (m_ControlType)
        {
            case ControlType.None:
                break;
            case ControlType.Character:
                m_PlayerController.InteractAction_Started();
                break;
            case ControlType.Boat:
                m_BoatController.InteractAction_Started();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SwitchController(m_ControlType == ControlType.Character ? ControlType.Boat : ControlType.Character);

        m_MoveInput = m_MoveAction.ReadValue<Vector2>();

        switch (m_ControlType)
        {
            case ControlType.None:
                break;
            case ControlType.Character:
                m_PlayerController.UpdateMoveInput(m_MoveInput, HasInput());
                break;
            case ControlType.Boat:
                m_BoatController.UpdateMoveInput(m_MoveInput, HasInput());
                break;
        }
    }

    public bool HasInput() => m_MoveInput.sqrMagnitude >= 0.01f;

    public void SwitchController(ControlType controlType)
    {
        switch (m_ControlType)
        {
            case ControlType.None:
                break;
            case ControlType.Character:
                m_PlayerController.StopControl();
                break;
            case ControlType.Boat:
                m_BoatController.StopControl();
                break;
        }

        m_ControlType = controlType;

        CameraManager.Instance.SwitchController(controlType);
    }
}