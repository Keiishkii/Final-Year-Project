using System;
using Unity.Barracuda;
using UnityEngine;

namespace _Barracuda
{
    // A container class for holding the neural network related classes, 
    [Serializable]
    public class Barracuda_Model
    {
        public NNModel model;
        private Model _runtimeModel;

        public IWorker worker;
        public Prediction prediction;


        // Generates the model and its workers.
        public void SetupModel()
        {
            _runtimeModel = ModelLoader.Load(model);
            worker = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.GPU);

            prediction = new Prediction();
        }


        // Cleans up the worker to avoid memory leaks
        private void OnDestroy()
        {
            worker?.Dispose();
        }
    }
}