using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class CustomInspectorPlayerController : Editor
{
    //
    //
    private bool m_bShowControllerInformations;

    //
    //
    public override void OnInspectorGUI()
    {
        //
        // Draw the built in inspector
        DrawDefaultInspector();

        //
        // Retrieve the custom inspector target as a PlayerController
        PlayerController oPlayerController = target as PlayerController;

        //
        // Display a WONDERFUL group
        m_bShowControllerInformations = EditorGUILayout.Foldout(m_bShowControllerInformations, "Controller Informations");

        if (m_bShowControllerInformations)
        {
            //
            // We can display the informations of the player controller
            EditorGUILayout.LabelField("Jump height : " + oPlayerController.m_fJumpHeight);
            float fDoubleJumpHeight = oPlayerController.m_fJumpHeight + (oPlayerController.m_fJumpHeight / 2f);
            EditorGUILayout.LabelField("Double jump height : " + fDoubleJumpHeight);
        }

        EditorGUILayout.EndFoldoutHeaderGroup();
    }
}
