#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class Player_Editor : Editor
{
    private bool _showBaseInspector;

    public override void OnInspectorGUI()
    {
        _showBaseInspector = EditorGUILayout.ToggleLeft("Show Base Inspector: ", _showBaseInspector);
        if (_showBaseInspector)
        {
            base.OnInspectorGUI();
        }
    }
}
#endif