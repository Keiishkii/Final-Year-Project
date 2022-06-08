using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Canvas controller used for the debug player controller, shows the FPS and other statics for the player
public class Debug_Canvas : MonoBehaviour
{
    [SerializeField] private TMP_Text _fpsDisplay;
    [SerializeField] private TMP_Text _rightActivationText;
    [SerializeField] private TMP_Text _leftActivationText;

    
    
    // Updates the text of the players right activation value.
    public void SetRightActivation(float value)
    {
        _rightActivationText.text = $"Right Activation: {value}";
    }
    
    // Updates the text of the players left activation value.
    public void SetLeftActivation(float value)
    {
        _leftActivationText.text = $"Left Activation: {value}";
    }

    // Updates the text displaying the current frame rate of the game.
    private void Update()
    {
        _fpsDisplay.text = $"FPS: {1 / Time.deltaTime}";
    }
}
