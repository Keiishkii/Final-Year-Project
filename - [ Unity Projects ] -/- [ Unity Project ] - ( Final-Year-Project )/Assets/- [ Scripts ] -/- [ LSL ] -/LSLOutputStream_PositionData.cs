using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

namespace _LSL
{
    // LSL output stream for writing position data across the network.
    public class LSLOutputStream_PositionData : LSLOutput<float>
    {
        // Generates an regular LSL stream to write position data too.
        private void Start()
        {
            float sampleRate = (1 / Time.fixedDeltaTime);

            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 3, sampleRate, channel_format_t.cf_float32);
            XMLElement chans = streamInfo.desc().append_child("channels");
            chans.append_child("channel").append_child_value("label", "X");
            chans.append_child("channel").append_child_value("label", "Y");
            chans.append_child("channel").append_child_value("label", "Z");

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new float[3];
        }

        // Creates and writes the position data to the network.
        private void FixedUpdate()
        {
            Vector3 pos = gameObject.transform.position;
            _currentSample[0] = pos.x;
            _currentSample[1] = pos.y;
            _currentSample[2] = pos.z;

            PushOutput();
        }
        
        // Pushes the data to the network
        private void PushOutput()
        {
            _outlet.push_sample(_currentSample);
        }
    }
}
