#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CrystalSequence
{
    // The custom inspector for the crystal sequencer, used to increment and decrement the active index of the sequence.
    // Without needing to go through the Crystal game objects.
    [CustomEditor(typeof(CrystalSequencer))]
    public class CrystalSequencer_Editor : Editor
    {
        private bool _baseGUIToggle;
        private int _activeIndex;
        
        
        // Draws the inspector, includes two buttons. One for incrementing the index and the other for decrementing it.
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