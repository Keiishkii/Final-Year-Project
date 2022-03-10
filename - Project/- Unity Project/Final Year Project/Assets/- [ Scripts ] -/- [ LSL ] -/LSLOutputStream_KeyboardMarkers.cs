using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLOutputStream_KeyboardMarkers : LSLOutput<string>
{
    void Start()
    {
        StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_string);
        XMLElement channels = streamInfo.desc().append_child("channels");
        channels.append_child("channel").append_child_value("label", "Marker");
        
        _outlet = new StreamOutlet(streamInfo);
        _currentSample = new string[1];
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keyCode))
                {
                    WriteMarker($"{keyCode}");
                }
            }
        }
    }

    private void WriteMarker(string marker)
    {
        _currentSample[0] = marker;
        _outlet.push_sample(_currentSample);
        
        Debug.Log($"KeyCode down: {marker}");
    }
}