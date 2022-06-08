using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

using _Barracuda;

// A VR interactable showcase for an XOR neural network.
// Displayed on top of a world space Canvas.
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



    // Sets up the network and renders the graphics of the world space canvas to the base state.
    private void Awake()
    {
        _modelXOR = new Barracuda_Model { model = _model };
        _modelXOR.SetupModel();
        
        CalculateOutputState();
        UpdateUIGraphics();
    }


    // Using the neural network for XOR data, predicts the result of a set of inputs controlled through the canvas buttons.
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

    // Redraws the UI based on the new data stored within the class.
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
    
    
    // On pressing a button on top of the canvas, the network is re-processed and the output is shown.
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

    // Clears up the worker
    private void OnDestroy()
    {
        _modelXOR.worker?.Dispose();
    }
}
