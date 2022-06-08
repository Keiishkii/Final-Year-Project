using System;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

namespace _Barracuda
{
    // The Prediction structure is used as an interface for a tensor, particularly the output tensor of a neural network.
    public struct Prediction
    { 
        private int _outputIndex;
        public int OutputIndex => _outputIndex;

        private float[] _outputs;
        public float[] Outputs => _outputs;

        // By passing in a tensor, this function search its data to return the output index of the tensor.
        public void SetOutput(ref Tensor tensor)
        {
            _outputs = tensor.AsFloats(); 
            _outputIndex = Array.IndexOf(_outputs, _outputs.Max());
        }
    }
}