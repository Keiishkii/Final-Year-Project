using System;
using System.Collections;
using System.Collections.Generic;
using _Barracuda;
using _LSL;
using Unity.Barracuda;
using UnityEngine;

// Showcase script for demonstrating the reading and processing of EEG motor imagery data.
// Does so by moving a game object left and right depending on what the networks predict to be the correct answer.
[RequireComponent(typeof(LSLInputStream_FloatArray))]
public class MotorImageryPlayerController : MonoBehaviour
{
    [SerializeField] private Barracuda_Model _model;
    private LSLInputStream_FloatArray _lslInputStreamRealTimeData;
    private Rigidbody _rigidbody;
    
    
    // Sets up the neural network and finds the components needed for accessing the data.
    private void Awake()
    {
        _lslInputStreamRealTimeData = GetComponent<LSLInputStream_FloatArray>();
        _rigidbody = GetComponent<Rigidbody>();
        
        _model.SetupModel();
    }

    
    // Each frame sampled data is fed into a neural network and the position of the game object is moved accordingly.
    private void Update()
    {
        List<float[]> samples = _lslInputStreamRealTimeData.Samples;
        if (samples.Count > 0)
        {
            foreach (float[] sampleSet in samples)
            {
                Tensor inputTensor = new Tensor(1, 1, 1, 64, sampleSet);
                Tensor outputTensor = _model.worker.Execute(inputTensor).PeekOutput();

                _model.prediction.SetOutput(ref outputTensor);

                switch (_model.prediction.OutputIndex)
                {
                    case 0:
                    {
                        Debug.Log("<color=#AAA>Standing Still</color>");
                    } break;
                    case 1:
                    {
                        Debug.Log("<color=#1BA>Moving Left</color>");
                        _rigidbody.AddForce(new Vector3(0.75f, 0, 0));
                    } break;
                    case 2:
                    {
                        Debug.Log("<color=#1BA>Moving Right</color>");
                        _rigidbody.AddForce(new Vector3(-0.75f, 0, 0));
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
