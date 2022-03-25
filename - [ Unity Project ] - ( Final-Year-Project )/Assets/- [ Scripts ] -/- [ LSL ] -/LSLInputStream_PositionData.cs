using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLInputStream_PositionData : LSLInput<float>
{
    private Transform _transform;


    
    private void Awake()
    {
        _transform = transform;
    }

    
    
    protected override void Update()
    {
        if (_streamInlet == null)
        {
            switch (_LSLStreamLookupType)
            {
                case LSLStreamLookup.NAME:
                {
                    _streamInfos = LSL.LSL.resolve_stream("name", _streamName, 1, 0);
                } break;
                case LSLStreamLookup.TYPE:
                {
                    _streamInfos = LSL.LSL.resolve_stream("type", _streamType, 1, 0);
                } break;
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
    
    public float[] GetCurrentSample()
    {
        return _sample;
    }

    private void Process(float[] newSample, double timeStamp)
    {
        if (newSample.Length >= 3)
        {
            _transform.position = new Vector3(newSample[0], newSample[1], newSample[2]);
        }
    }
}