using System;
using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

namespace _LSL
{
    public class LSLOutputStream_StartAndStop : LSLOutput<int>
    {
        private void Start()
        {
            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_int32);

            XMLElement channels = streamInfo.desc().append_child("channels");
            channels.append_child("channel").append_child_value("label", "Marker");

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new int[1];
        }

        private void Update()
        {

        }



        protected override void PushOutput()
        {
            _outlet.push_sample(_currentSample);
        }
    }
}