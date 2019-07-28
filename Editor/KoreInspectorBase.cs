using UnityEngine;
using UnityEditor;

namespace KoreInspector
{
    public abstract class KoreInspectorBase : Editor
    {
        /// <summary>
        /// Does the inspector override the default Unity inspector?
        /// </summary>
        public bool overrideDefault
        {
            get { return m_OverrideDefault; }
            private set { m_OverrideDefault = value; }
        }
        private bool m_OverrideDefault;

        protected virtual void OnEnable()
        {
            RefreshOverride();
            KoreInspectorEditorWindow.onInspectorOverrideChanged += RefreshOverride;
        }

        public sealed override void OnInspectorGUI()
        {
            if (overrideDefault)
            {
                OnOverrideInspectorGUI();
            }
            else
            {
                OnDefaultInspectorGUI();
            }
        }

        protected virtual void OnDisable()
        {
            KoreInspectorEditorWindow.onInspectorOverrideChanged -= RefreshOverride;
        }

        protected abstract void OnOverrideInspectorGUI();
        protected abstract void OnDefaultInspectorGUI();

        protected virtual void RefreshOverride()
        {
            if (!EditorPrefs.HasKey(KoreInspectorSettings.GetPrefsKey(this.GetType())))
            {
                overrideDefault = true;
            }
            else
            {
                overrideDefault = EditorPrefs.GetBool(KoreInspectorSettings.GetPrefsKey(this.GetType()));
            }
        }
    }
}
