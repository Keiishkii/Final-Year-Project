#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace CrystalSequence
{
    // Crystal Sequence Mini-game controllers custom editor, used as a way of inputting the colours of the index without needing to go through the crystal game objects.
    // Using this allows for the testing of the sequences inputs as well as if the correct actions are taken inn the case of an incorrect input.
    [CustomEditor(typeof(MiniGameController_CrystalSequence))]
    public class MiniGameController_CrystalSequence_Editor : Editor
    {
        private bool _baseGUIToggle;
        
        
        // Draws the inspector with a list of buttons, each for the potential colours in the sequence.
        public override void OnInspectorGUI()
        {
            MiniGameController_CrystalSequence targetScript = (MiniGameController_CrystalSequence) target;

            
            if (Application.isPlaying && targetScript.gameObject.scene.name != null)
            {
                if (GUILayout.Button("Activate: RED"))
                {
                    targetScript.ActivatedCrystal(CrystalColours_Enum.RED);
                }
                if (GUILayout.Button("Activate: ORANGE"))
                {
                    targetScript.ActivatedCrystal(CrystalColours_Enum.ORANGE);
                }
                if (GUILayout.Button("Activate: BLUE"))
                {
                    targetScript.ActivatedCrystal(CrystalColours_Enum.BLUE);
                }
                if (GUILayout.Button("Activate: GREEN"))
                {
                    targetScript.ActivatedCrystal(CrystalColours_Enum.GREEN);
                }
                if (GUILayout.Button("Activate: PURPLE"))
                {
                    targetScript.ActivatedCrystal(CrystalColours_Enum.PURPLE);
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