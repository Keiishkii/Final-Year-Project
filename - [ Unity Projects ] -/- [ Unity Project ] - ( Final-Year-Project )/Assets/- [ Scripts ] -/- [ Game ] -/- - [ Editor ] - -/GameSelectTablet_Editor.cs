#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// Custom inspector for the game mode selection tablets, used to interact with them out side of in game interactions.
[CustomEditor(typeof(GameSelectTablet))]
public class GameSelectTablet_Editor : Editor
{
    private bool _baseGUIToggle;
    
    
    // Redraws the inspector 
    public override void OnInspectorGUI()
    {
        GameSelectTablet targetScript = (GameSelectTablet) target;
        
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
#endif