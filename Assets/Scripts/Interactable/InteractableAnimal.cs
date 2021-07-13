using UnityEngine;

public class InteractableAnimal : MonoBehaviour, IInteractable
{
    [SerializeField] private Material m_DefaultMaterial = null;
    [SerializeField] private Material m_InteractableMaterial = null;
    [SerializeField] private Renderer m_Renderer = null;

    public void OnEnter()
    {
        Debug.Log("OnEnter", this);
        m_Renderer.sharedMaterial = m_InteractableMaterial;
    }

    public void OnInteract()
    {
        Debug.Log("OnInteract", this);
    }

    public void OnExit()
    {
        Debug.Log("OnExit", this);
        m_Renderer.sharedMaterial = m_DefaultMaterial;
    }
}