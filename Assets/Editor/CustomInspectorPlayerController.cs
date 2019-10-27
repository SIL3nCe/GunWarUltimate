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
    private bool m_bShowDebugInformations;

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

        //
        // Draw the debug informations (only when we are playing in the editor)
        if (EditorApplication.isPlaying)
        {
            DrawDebugInformations(oPlayerController);
        }
    }

    public void DrawDebugInformations(PlayerController oTarget)
    {
        //
        // Start the debug foldout group
        m_bShowDebugInformations = EditorGUILayout.Foldout(m_bShowDebugInformations, "Debug");

        if (m_bShowDebugInformations)
        {
            //
            // We display the player ground state
            if (oTarget.IsGrounded())
            {
                EditorGUILayout.LabelField("Grounded : True");
            }
            else
            {
                EditorGUILayout.LabelField("Grounded : False");
            }

            //
            // We display the informations about double jump
            if (oTarget.CanDoubleJump())
            {
                EditorGUILayout.LabelField("Double jump available : True");
            }
            else
            {
                EditorGUILayout.LabelField("Double jump available : False");
            }

            //
            // We display if the player is wall hanging, and if so it's wall hang direction
            if (oTarget.IsWallHanging())
            {
                EditorGUILayout.LabelField("Wall hanging : True");

                int iWallHangDirection = oTarget.GetWallHangDirection();

                if (iWallHangDirection == 1)
                {
                    EditorGUILayout.LabelField("Hang direction : +X");
                }
                else if (iWallHangDirection == -1)
                {
                    EditorGUILayout.LabelField("Hang direction : -X");
                }
                else
                {
                    EditorGUILayout.LabelField("Hang direction : NONE");
                }
            }
        }
    }
}
