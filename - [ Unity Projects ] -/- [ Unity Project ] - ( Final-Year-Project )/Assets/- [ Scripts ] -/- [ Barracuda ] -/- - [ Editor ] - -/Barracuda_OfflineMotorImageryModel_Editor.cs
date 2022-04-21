#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Barracuda
{
    [CustomEditor(typeof(Barracuda_OfflineMotorImageryModel))]
    public class Barracuda_OfflineMotorImageryModel_Editor : Editor
    {
        private SerializedProperty _model;
        
        private string _outputLabel = "~";
        
        
        
        private void OnEnable()
        {
            _model = serializedObject.FindProperty("model");
        }
        
        
        
        public override void OnInspectorGUI()
        {
            Barracuda_OfflineMotorImageryModel targetScript = (Barracuda_OfflineMotorImageryModel) target;
            EditorGUILayout.PropertyField(_model);

            if (Application.isPlaying)
            {
                EditorGUILayout.Space(5);
                if (GUILayout.Button("Calculate"))
                {
                    targetScript.model.SetupModel();

                    List<float> inputFloats = new List<float>();
                    for (int i = 0; i < 64; i++)
                    {
                        inputFloats.Add(Random.Range(-165.0f, 165.0f));
                    }
                    
                    Tensor inputTensor = new Tensor(1, 1, 1, 64, inputFloats.ToArray());
                    Tensor outputTensor = targetScript.model.worker.Execute(inputTensor).PeekOutput();

                    targetScript.model.prediction.SetOutput(outputTensor);
                    //_outputLabel = $"{targetScript.model.prediction.Outputs[0] + ", " + targetScript.model.prediction.Outputs[1] + ", "+ targetScript.model.prediction.Outputs[2]}";
                    _outputLabel = $"{targetScript.model.prediction.OutputIndex}";

                    inputTensor.Dispose();
                    outputTensor.Dispose();
                    targetScript.model.worker?.Dispose();
                }

                EditorGUILayout.Space(5);

                GUIStyle outputLabel = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter, fontSize = 16};
                EditorGUILayout.LabelField(_outputLabel, outputLabel);
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif