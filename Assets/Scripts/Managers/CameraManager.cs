using UnityEngine;
using GameDevStack.Patterns;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    [SerializeField] private GameObject m_CharacterCamera = null;
    [SerializeField] private GameObject m_BoatCamera = null;

    public void SwitchController(ControlType controlType)
    {
        switch (controlType)
        {
            case ControlType.None:
                break;
            case ControlType.Character:
                m_CharacterCamera.SetActive(true);
                m_BoatCamera.SetActive(false);
                break;
            case ControlType.Boat:
                m_CharacterCamera.SetActive(false);
                m_BoatCamera.SetActive(true);
                break;
        }
    }
}