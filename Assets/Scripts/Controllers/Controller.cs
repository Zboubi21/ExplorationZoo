using UnityEngine;

public class Controller : MonoBehaviour
{
    protected bool m_HasInput;
    protected Vector3 m_InputDirection;
    protected Vector3 m_LastInputDirection;

    public virtual void InteractAction_Started() { }

    public virtual void UpdateMoveInput(Vector2 moveInput, bool hasInput)
    {
        m_InputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        m_HasInput = hasInput;
    }

    public virtual void StopControl()
    {
        m_InputDirection = Vector3.zero;
        m_HasInput = false;
    }
}