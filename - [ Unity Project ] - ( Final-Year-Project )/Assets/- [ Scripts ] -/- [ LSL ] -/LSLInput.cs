using LSL;
using UnityEngine;

public abstract class LSLInput<T> : MonoBehaviour
{
    [SerializeField] protected string _streamName = "A";
    [SerializeField] protected string _streamType = "A";

    [SerializeField] protected LSLStreamLookup _LSLStreamLookupType = LSLStreamLookup.NAME;
    
    protected int _channelCount = 0;
    
    protected StreamInfo[] _streamInfos;
    protected StreamInlet _streamInlet;
    protected T[] _sample;


    
    protected abstract void Update();
}