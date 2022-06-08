using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The controller for the screen space canvas that displays the blink indicator and cross-hair.
public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject _eyeOpenPanel;
    [SerializeField] private GameObject _eyeClosedPanel;
    private bool _blinking;
    
    private Player _player;

    
    
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        
        _blinking = _player.blinking;
        
        _eyeOpenPanel.SetActive(!_blinking);
        _eyeClosedPanel.SetActive(_blinking);
    }


    // Each frame the state of a players eye movement is retrieved from the player class, following this it is then displayed to the screen by enabling and disabling images. 
    private void Update()
    {
        if (_blinking != _player.blinking)
        {
            _blinking = !_blinking;
            
            _eyeOpenPanel.SetActive(!_blinking);
            _eyeClosedPanel.SetActive(_blinking);
        }
    }
}
