using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace KoreInspector
{
    public class KoreInspectorSettings
    {
        /// <summary>
        /// Returns the editor preference key string of the given inspector type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetPrefsKey(Type type)
        {
            return "KoreInspector." + type.ToString();
        }
    }

    public class KoreInspectorEditorWindow : EditorWindow
    {
        private static bool s_EnableKoreTransformInpsector;
        private static bool s_EnableKoreRectTransformInspector;
        private static List<Type> s_KoreInspectorSubTypes = new List<Type>();
        private static bool[] s_InpsectorOverrides;
        public delegate void OnInspectorOverrideChanged();
        public static OnInspectorOverrideChanged onInspectorOverrideChanged;

        private static void FindAllKoreInspectors()
        {
            Type koreInspectorType = typeof(KoreInspectorBase);
            Type[] allTypes = koreInspectorType.Assembly.GetTypes();
            s_KoreInspectorSubTypes.Clear();
            for (int i = 0; i < allTypes.Length; i++)
            {
                if (allTypes[i].IsSubclassOf(koreInspectorType) && !allTypes[i].IsAbstract)
                {
                    s_KoreInspectorSubTypes.Add(allTypes[i]);
                }
            }

            // Find overridden inspectors from editor preferences
            s_InpsectorOverrides = new bool[s_KoreInspectorSubTypes.Count];
            for (int i = 0; i < s_KoreInspectorSubTypes.Count; i++)
            {
                s_InpsectorOverrides[i] = EditorPrefs.GetBool(KoreInspectorSettings.GetPrefsKey(s_KoreInspectorSubTypes[i]));
            }
        }


        [MenuItem("Window/Kore/Inspector Settings")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(KoreInspectorEditorWindow));
            window.titleContent = new GUIContent("Kore Inspector Settings");
        }

        void OnEnable()
        {
            FindAllKoreInspectors();
        }

        void OnGUI()
        {
            for (int i = 0; i < s_KoreInspectorSubTypes.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                s_InpsectorOverrides[i] = EditorGUILayout.Toggle(ToTitleCase(s_KoreInspectorSubTypes[i].Name), s_InpsectorOverrides[i]);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(this, "Changed override");
                    EditorPrefs.SetBool(KoreInspectorSettings.GetPrefsKey(s_KoreInspectorSubTypes[i]), s_InpsectorOverrides[i]);
                    onInspectorOverrideChanged();
                }
            }
        }

        void OnDestroy()
        {
        }

        private static string ToTitleCase(string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + m.Value[1]);
        }
    }

}
