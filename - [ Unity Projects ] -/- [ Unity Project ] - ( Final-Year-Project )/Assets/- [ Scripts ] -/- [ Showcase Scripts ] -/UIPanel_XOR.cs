using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

using _Barracuda;

public class UIPanel_XOR : MonoBehaviour
{
    [SerializeField] private List<Image> _inputAImages;
    [SerializeField] private List<Image> _inputBImages;
    [SerializeField] private Image _outputImage;
    [SerializeField] private TMP_Text _outputText;
    
    [SerializeField] private Color _activeColour;
    [SerializeField] private Color _nonactiveColour;

    [SerializeField] private NNModel _model;
    private Barracuda_Model _modelXOR;

    private bool _inputAState;
    private bool _inputBState;
    private bool _outputState;




    private void Awake()
    {
        _modelXOR = new Barracuda_Model { model = _model };
        _modelXOR.SetupModel();
        
        CalculateOutputState();
        UpdateUIGraphics();
    }



    private void CalculateOutputState()
    {
        List<float> inputFloats = new List<float> {_inputAState ? 0 : 1, _inputBState ? 0 : 1};
        
        Tensor inputTensor = new Tensor(1, 1, 1, 2, inputFloats.ToArray());
        Tensor outputTensor = _modelXOR.worker.Execute(inputTensor).PeekOutput();
            
        _modelXOR.prediction.SetOutput(ref outputTensor);
        _outputState = (_modelXOR.prediction.Outputs[0] > 0.5f);
                
        inputTensor.Dispose();
        outputTensor.Dispose();
    }

    private void UpdateUIGraphics()
    {
        Color inputA = (_inputAState) ? _activeColour : _nonactiveColour;
        foreach (Image image in _inputAImages)
        {
            image.color = inputA;
        }
        
        Color inputB = (_inputBState) ? _activeColour : _nonactiveColour;
        foreach (Image image in _inputBImages)
        {
            image.color = inputB;
        }
        
        _outputImage.color = (_outputState) ? _activeColour : _nonactiveColour;
        _outputText.text = (_outputState) ? "ON" : "OFF";
    }
    
    
    
    public void InputPressed(int inputID)
    {
        switch (inputID)
        {
            case 0:
            {
                _inputAState = !_inputAState;
                
                CalculateOutputState();
                UpdateUIGraphics();
            } break;
            case 1:
            {
                _inputBState = !_inputBState;
                
                CalculateOutputState();
                UpdateUIGraphics();
            } break;
        }
    }

    private void OnDestroy()
    {
        _modelXOR.worker?.Dispose();
    }
}
