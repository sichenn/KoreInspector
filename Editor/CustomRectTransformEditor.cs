using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace TP.Tools
{
	[CustomEditor(typeof(RectTransform), true)] 
	public class CustomRectTransformEditor : Editor
	{
		private Editor m_editorInstance;
		private RectTransform m_rectTransformTarget;
		private Vector2 m_rectSize;

		private void OnEnable()
		{
			Assembly ass = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			Type rtEditor = ass.GetType("UnityEditor.RectTransformEditor");
			m_editorInstance = CreateEditor(target, rtEditor);
			m_rectTransformTarget = target as RectTransform;
		}
	
		public override void OnInspectorGUI()
		{
			m_editorInstance.OnInspectorGUI();
			DisplayRectSize();
		}

		private void DisplayRectSize()
		{
			m_rectSize = m_rectTransformTarget.rect.size;
			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.Vector2Field("Size", m_rectSize);
			EditorGUI.EndDisabledGroup();
		}
	}

}
