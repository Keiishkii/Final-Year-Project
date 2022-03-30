#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine;

namespace _Barracuda
{
    [CustomEditor(typeof(Barracuda_XORModel))]
    public class Barracuda_XORModel_Editor : Editor
    {
        private SerializedProperty _model;
        private int _inputA, _inputB;

        private string _outputLabel = "~";

        

        private void OnEnable()
        {
            _model = serializedObject.FindProperty("model");
        }



        public override void OnInspectorGUI()
        {
            Barracuda_XORModel targetScript = (Barracuda_XORModel) target;
            EditorGUILayout.PropertyField(_model);

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            _inputA = EditorGUILayout.IntField("Input A", _inputA);
            _inputB = EditorGUILayout.IntField("Input B", _inputB);
            EditorGUILayout.EndHorizontal();

            if (Application.isPlaying)
            {
                EditorGUILayout.Space(5);
                if (GUILayout.Button("Calculate"))
                {
                    targetScript.model.SetupModel();
                    List<float> inputFloats = new List<float> {_inputA, _inputB};

                    Tensor inputTensor = new Tensor(1, 1, 1, 2, inputFloats.ToArray());
                    Tensor outputTensor = targetScript.model.worker.Execute(inputTensor).PeekOutput();

                    targetScript.model.prediction.SetOutput(outputTensor);
                    _outputLabel = $"{(targetScript.model.prediction.Outputs[0] > 0.5f)}";

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