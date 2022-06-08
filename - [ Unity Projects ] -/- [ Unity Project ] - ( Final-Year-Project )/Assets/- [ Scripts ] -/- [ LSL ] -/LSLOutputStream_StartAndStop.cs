using System;
using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

namespace _LSL
{
    // LSL output stream for sending marker data across the network, with the phrases Start and Stop (for interacting with EEGO Sport's software).
    public class LSLOutputStream_StartAndStop : LSLOutput<int>
    {
        // Generates an irregular network to write data over.
        private void Start()
        {
            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_int32);

            XMLElement channels = streamInfo.desc().append_child("channels");
            channels.append_child("channel").append_child_value("label", "Marker");

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new int[1];
        }

        
        // Pushes the marker data to the LSL stream.
        private void PushOutput()
        {
            _outlet.push_sample(_currentSample);
        }
    }
}