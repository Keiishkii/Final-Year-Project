#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Barracuda_EEGMotorImageryModel))]
public class Barracuda_EEGMotorImageryModel_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif