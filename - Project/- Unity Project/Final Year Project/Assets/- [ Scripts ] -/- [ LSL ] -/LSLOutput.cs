using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public abstract class LSLOutput<T> : MonoBehaviour
{
    [SerializeField] protected string _streamName = "A";
    [SerializeField] protected string _streamType = "A";
    
    protected StreamOutlet _outlet;
    protected T[] _currentSample;

    protected abstract void PushOutput();
}