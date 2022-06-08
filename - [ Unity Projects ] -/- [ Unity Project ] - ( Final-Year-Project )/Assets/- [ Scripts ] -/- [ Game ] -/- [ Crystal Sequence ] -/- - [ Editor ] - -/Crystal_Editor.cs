#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CrystalSequence
{
    // Custom inspector for the crystal class, used to activate a crystal without needing to use the in game interaction.
    [CustomEditor(typeof(Crystal))]
    public class Crystal_Editor : Editor
    {
        private bool _baseGUIToggle;
        
        
        // Draws the inspector with a button for calling the crystals activation function.
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

        // Toggle for redrawing the base inspector of the GUI.
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