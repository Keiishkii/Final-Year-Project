#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CrystalSequence
{
    [CustomEditor(typeof(CrystalSequencer))]
    public class CrystalSequencer_Editor : Editor
    {
        private bool _baseGUIToggle;
        private int _activeIndex;
        
        
        
        public override void OnInspectorGUI()
        {
            CrystalSequencer targetScript = (CrystalSequencer) target;

            if (Application.isPlaying && targetScript.gameObject.scene.name != null)
            {
                _activeIndex = EditorGUILayout.IntField("Active Index: ", _activeIndex);
                if (GUILayout.Button($"Lock Active Index"))
                {
                    targetScript.LockElement(_activeIndex);
                }

                if (GUILayout.Button($"Unlock Active Index"))
                {
                    targetScript.UnlockElement(_activeIndex);
                }
            }

            DrawBaseInspector();
        }
        
        private void DrawBaseInspector()
        {
            GUILayout.Space(10);
            
            if (GUILayout.Button($"{((!_baseGUIToggle) ? ("Show") : ("Hide"))} base GUI."))
            {
                _baseGUIToggle = !_baseGUIToggle;
            }
            
            GUILayout.Space(10);

            if (_baseGUIToggle) base.OnInspectorGUI();
        }
    }
}
#endif