using System.Collections;
using System.Collections.Generic;
using _LSL;
using LSL;
using UnityEngine;

namespace _LSL
{
    public class LSLInputStream_FloatArray : LSLInput<float>
    {
        private readonly List<float[]> _samples = new List<float[]>();
        public List<float[]> Samples => _samples;



        protected override void Update()
        {
            if (_streamInlet == null)
            {
                switch (_LSLStreamLookupType)
                {
                    case LSLStreamLookup.NAME:
                    {
                        _streamInfos = LSL.LSL.resolve_stream("name", _streamName, 1, 0);
                    }
                        break;
                    case LSLStreamLookup.TYPE:
                    {
                        _streamInfos = LSL.LSL.resolve_stream("type", _streamType, 1, 0);
                    }
                        break;
                }

                if (_streamInfos.Length > 0)
                {
                    _streamInlet = new StreamInlet(_streamInfos[0]);
                    _channelCount = _streamInlet.info().channel_count();
                    _streamInlet.open_stream();
                }
            }

            if (_streamInlet != null)
            {
                _samples.Clear();
                _sample = new float[_channelCount];
                double lastTimeStamp = _streamInlet.pull_sample(_sample, 0.0f);
                if (lastTimeStamp != 0.0)
                {
                    Process(_sample, lastTimeStamp);
                    while ((lastTimeStamp = _streamInlet.pull_sample(_sample, 0.0f)) != 0)
                    {
                        Process(_sample, lastTimeStamp);
                    }
                }
            }
            else
            {
                Debug.Log("StreamInput is still null");
            }
        }

        private void Process(float[] newSample, double timeStamp)
        {
            _samples.Add(newSample);
        }
    }
}