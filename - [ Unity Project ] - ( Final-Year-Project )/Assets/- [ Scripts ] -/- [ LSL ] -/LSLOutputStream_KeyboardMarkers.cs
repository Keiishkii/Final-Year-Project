using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using UnityEngine.InputSystem;

public class LSLOutputStream_KeyboardMarkers : LSLOutput<string>
{
    private InputActions _inputActions;


    private void Awake()
    {
        _inputActions = new InputActions();
        _inputActions.Keyboard.W.performed += ctx => PushOutput();
    }


    private void Start()
    {
        StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_string);
        
        XMLElement channels = streamInfo.desc().append_child("channels");
            channels.append_child("channel").append_child_value("label", "Marker");
        
        _outlet = new StreamOutlet(streamInfo);
        _currentSample = new string[1];
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }


    protected override void PushOutput()
    {
        _currentSample[0] = "Hello";
        
        Debug.Log(_currentSample);
        _outlet.push_sample(_currentSample);
    }
}