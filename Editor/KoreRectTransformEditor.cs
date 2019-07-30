using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace KoreInspector
{
	/// <summary>
	/// Currently this is the same as the default editor (￣y▽,￣)╭ 
	/// </summary>
	[CustomEditor(typeof(RectTransform), true)] 
	public class KoreRectTransformEditor : KoreInspectorBase
	{
		private Editor m_EditorInstance;
		private RectTransform m_RectTransform;
		private Vector2 m_rectSize;

		protected override void OnEnable()
		{
			base.OnEnable();
			Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
			Type rtEditor = assembly.GetType("UnityEditor.RectTransformEditor");
			m_EditorInstance = CreateEditor(target, rtEditor);
			m_RectTransform = target as RectTransform;
		}
	
		protected override void OnDefaultInspectorGUI()
		{
            m_EditorInstance.OnInspectorGUI();
        }

		protected override void OnOverrideInspectorGUI()
		{
			m_EditorInstance.OnInspectorGUI();
		}
	}

}
