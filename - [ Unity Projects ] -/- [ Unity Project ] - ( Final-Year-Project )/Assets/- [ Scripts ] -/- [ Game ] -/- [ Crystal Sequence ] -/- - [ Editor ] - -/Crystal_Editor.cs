#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CrystalSequence
{
    [CustomEditor(typeof(Crystal))]
    public class Crystal_Editor : Editor
    {
        private bool _baseGUIToggle;
        
        
        
        public override void OnInspectorGUI()
        {
            Crystal targetScript = (Crystal) target;

            if (Application.isPlaying && targetScript.gameObject.scene.name != null)
            {
                if (GUILayout.Button("Activate"))
                {
                    targetScript.Activate();
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