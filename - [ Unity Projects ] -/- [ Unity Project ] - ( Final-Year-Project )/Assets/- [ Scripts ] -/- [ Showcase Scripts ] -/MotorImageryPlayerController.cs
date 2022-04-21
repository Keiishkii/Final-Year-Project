using System;
using System.Collections;
using System.Collections.Generic;
using _Barracuda;
using _LSL;
using Unity.Barracuda;
using UnityEngine;

[RequireComponent(typeof(LSLInputStream_EEGRealTimeData))]
public class MotorImageryPlayerController : MonoBehaviour
{
    [SerializeField] private Barracuda_Model _model;
    private LSLInputStream_EEGRealTimeData _lslInputStreamRealTimeData;
    private Rigidbody _rigidbody;
    
    
    
    private void Awake()
    {
        _lslInputStreamRealTimeData = GetComponent<LSLInputStream_EEGRealTimeData>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _model.SetupModel();
    }

    
    
    private void Update()
    {
        List<float[]> samples = _lslInputStreamRealTimeData.Samples;
        if (samples.Count > 0)
        {
            foreach (float[] sampleSet in samples)
            {
                Tensor inputTensor = new Tensor(1, 1, 1, 64, sampleSet);
                Tensor outputTensor = _model.worker.Execute(inputTensor).PeekOutput();

                _model.prediction.SetOutput(outputTensor);

                switch (_model.prediction.OutputIndex)
                {
                    case 0:
                    {
                        Debug.Log("<color=#AAA>Standing Still</color>");
                    } break;
                    case 1:
                    {
                        Debug.Log("<color=#1BA>Moving Left</color>");
                        _rigidbody.AddForce(new Vector3(0.25f, 0, 0));
                    } break;
                    case 2:
                    {
                        Debug.Log("<color=#1BA>Moving Right</color>");
                        _rigidbody.AddForce(new Vector3(-0.25f, 0, 0));
                    } break;
                }
                
                
                inputTensor.Dispose();
                outputTensor.Dispose();
            }
        }
    }

    

    private void OnDestroy()
    {
        _model.worker?.Dispose();
    }
}
