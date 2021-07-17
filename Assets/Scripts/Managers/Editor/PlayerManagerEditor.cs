using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
public class PlayerManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_ControlType"));

        if (serializedObject.ApplyModifiedProperties())
        {
            PlayerManager playerManager = (PlayerManager)target;
            playerManager.SwitchController(playerManager.ControlType);
        }
    }
}