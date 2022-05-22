using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
