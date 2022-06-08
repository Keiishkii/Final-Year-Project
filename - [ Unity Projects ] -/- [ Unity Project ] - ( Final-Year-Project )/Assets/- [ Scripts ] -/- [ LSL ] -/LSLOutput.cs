using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using UnityEngine.InputSystem;

namespace _LSL
{
    // Abstract class used for writing LSL streams. Stores the values it will be writing, and the name of the stream and its description.
    public abstract class LSLOutput<T> : MonoBehaviour
    {
        [SerializeField] protected string _streamName = "A";
        [SerializeField] protected string _streamType = "A";

        protected StreamOutlet _outlet;
        protected T[] _currentSample;
    }
}