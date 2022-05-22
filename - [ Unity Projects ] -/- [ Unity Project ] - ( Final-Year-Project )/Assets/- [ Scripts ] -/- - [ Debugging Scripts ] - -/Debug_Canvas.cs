using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debug_Canvas : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsDisplay;
    [SerializeField] private TMP_Text _rightActivationText;
    [SerializeField] private TMP_Text _leftActivationText;

    public void SetRightActivation(float value)
    {
        _rightActivationText.text = $"Right Activation: {value}";
    }
    
    public void SetLeftActivation(float value)
    {
        _leftActivationText.text = $"Left Activation: {value}";
    }

    private void Update()
    {
        _fpsDisplay.text = $"FPS: {1 / Time.deltaTime}";
    }
}
