using System;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;

namespace _Barracuda
{
        public struct Prediction
        {
                private int _outputIndex;
                public int OutputIndex => _outputIndex;

                private float[] _outputs;
                public float[] Outputs => _outputs;

                public void SetOutput(Tensor tensor)
                {
                        _outputs = tensor.AsFloats();
                        _outputIndex = Array.IndexOf(_outputs, _outputs.Max());
                }
        }
}