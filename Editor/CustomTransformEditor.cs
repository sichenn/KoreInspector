using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine;
using KoreInspector;
using UnityEditor;

namespace UnityEditor
{
    // Original Author: Cobo3, https://forum.unity.com/threads/extending-instead-of-replacing-built-in-inspectors.407612/
    // Edited by Sichen Liu
    [CustomEditor(typeof(Transform), true), CanEditMultipleObjects]
    public class CustomTransformInspector : KoreInspectorBase
    {
        Editor m_DefaultEditor;
        Transform m_Transform;

        protected override void OnEnable()
        {
            base.OnEnable();
            //When this inspector is created, also create the built-in inspector
            m_DefaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
            m_Transform = target as Transform;
        }

        protected override void OnDefaultInspectorGUI()
        {
            // DefaultTransformInspector();
            m_DefaultEditor.OnInspectorGUI();
        }

        protected override void OnOverrideInspectorGUI()
        {
            // Show local space 
            EditorGUILayout.LabelField("Local Space", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            OnDefaultInspectorGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.Width(8));
            if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"), EditorStyles.toolbarButton))
            {
                Undo.RecordObject(m_Transform, "Reset position");
                m_Transform.localPosition = Vector3.zero;
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"), EditorStyles.toolbarButton))
            {
                Undo.RecordObject(m_Transform, "Reset rotation");
                m_Transform.localRotation = Quaternion.identity;
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"), EditorStyles.toolbarButton))
            {
                Undo.RecordObject(m_Transform, "Reset scale");
                m_Transform.localScale = Vector3.one;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            // Show world space
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("World Space", EditorStyles.boldLabel);
            GUI.enabled = false;
            EditorGUILayout.Vector3Field("Position", m_Transform.position);
            EditorGUILayout.Vector3Field("Rotation", m_Transform.eulerAngles);
            EditorGUILayout.Vector3Field("Scale", m_Transform.lossyScale);
            GUI.enabled = true;
        }

        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = m_DefaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null)
                disableMethod.Invoke(m_DefaultEditor, null);
            DestroyImmediate(m_DefaultEditor);
        }

        // private void DefaultTransformInspector()
        // {
        //     bool didPositionChange = false;
        //     bool didRotationChange = false;
        //     bool didScaleChange = false;

        //     // Watch for changes.
        //     //  1)  Float values are imprecise, so floating point error may cause changes
        //     //      when you've not actually made a change.
        //     //  2)  This allows us to also record an undo point properly since we're only
        //     //      recording when something has changed.

        //     // Store current values for checking later
        //     Vector3 initialLocalPosition = m_Transform.localPosition;
        //     Vector3 initialLocalEuler = m_Transform.localEulerAngles;
        //     Vector3 initialLocalScale = m_Transform.localScale;

        //     EditorGUILayout.BeginHorizontal();
        //     EditorGUI.BeginChangeCheck();

        //     m_LocalPosition = EditorGUILayout.Vector3Field("Position", m_Transform.localPosition);
        //     if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"),
        //             GUILayout.Width(EditorGUIUtility.singleLineHeight * 1.5f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        //     {
        //         m_LocalPosition = Vector3.zero;
        //         didPositionChange = true;
        //     }
        //     if (EditorGUI.EndChangeCheck())
        //         didPositionChange = true;


        //     EditorGUILayout.EndHorizontal();

        //     EditorGUILayout.BeginHorizontal();
        //     EditorGUI.BeginChangeCheck();

        //     m_LocalEulerAngles = EditorGUILayout.Vector3Field("Rotation", m_Transform.localEulerAngles);
        //     if (EditorGUI.EndChangeCheck())
        //         didRotationChange = true;
        //     if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"),
        //             GUILayout.Width(EditorGUIUtility.singleLineHeight * 1.5f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        //     {
        //         m_LocalEulerAngles = Vector3.zero;
        //         didRotationChange = true;
        //     }
        //     EditorGUILayout.EndHorizontal();

        //     EditorGUILayout.BeginHorizontal();
        //     EditorGUI.BeginChangeCheck();
        //     m_LocalScale = EditorGUILayout.Vector3Field("Scale", m_Transform.localScale);
        //     if (EditorGUI.EndChangeCheck())
        //         didScaleChange = true;
        //     if (GUILayout.Button(EditorGUIUtility.IconContent("RotateTool"),
        //             GUILayout.Width(EditorGUIUtility.singleLineHeight * 1.5f), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
        //     {
        //         m_LocalScale = Vector3.one;
        //         didScaleChange = true;
        //     }
        //     EditorGUILayout.EndHorizontal();

        //     // Apply changes with record undo
        //     if (didPositionChange || didRotationChange || didScaleChange)
        //     {
        //         Undo.RecordObject(m_Transform, m_Transform.name);

        //         if (didPositionChange)
        //             m_Transform.localPosition = m_LocalPosition;

        //         if (didRotationChange)
        //             m_Transform.localEulerAngles = m_LocalEulerAngles;

        //         if (didScaleChange)
        //             m_Transform.localScale = m_LocalScale;
        //         this.Repaint();
        //     }

        //     // Since BeginChangeCheck only works on the selected object
        //     // we need to manually apply transform changes to all selected objects.
        //     Transform[] selectedTransforms = Selection.transforms;
        //     if (selectedTransforms.Length > 1)
        //     {
        //         foreach (var item in selectedTransforms)
        //         {
        //             if (didPositionChange || didRotationChange || didScaleChange)
        //                 Undo.RecordObject(item, item.name);

        //             if (didPositionChange)
        //             {
        //                 item.localPosition = ApplyChangesOnly(
        //                     item.localPosition, initialLocalPosition, m_Transform.localPosition);
        //             }

        //             if (didRotationChange)
        //             {
        //                 item.localEulerAngles = ApplyChangesOnly(
        //                     item.localEulerAngles, initialLocalEuler, m_Transform.localEulerAngles);
        //             }

        //             if (didScaleChange)
        //             {
        //                 item.localScale = ApplyChangesOnly(
        //                     item.localScale, initialLocalScale, m_Transform.localScale);
        //             }

        //         }
        //     }
        // }

        private Vector3 ApplyChangesOnly(Vector3 toApply, Vector3 initial, Vector3 changed)
        {
            if (!Mathf.Approximately(initial.x, changed.x))
                toApply.x = m_Transform.localPosition.x;

            if (!Mathf.Approximately(initial.y, changed.y))
                toApply.y = m_Transform.localPosition.y;

            if (!Mathf.Approximately(initial.z, changed.z))
                toApply.z = m_Transform.localPosition.z;

            return toApply;
        }
    }
}
