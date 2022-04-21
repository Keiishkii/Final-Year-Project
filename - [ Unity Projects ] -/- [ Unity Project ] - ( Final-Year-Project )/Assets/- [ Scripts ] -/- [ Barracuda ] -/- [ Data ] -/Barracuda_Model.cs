using System;
using Unity.Barracuda;
using UnityEngine;

namespace _Barracuda
{
    [Serializable]
    public class Barracuda_Model
    {
        public NNModel model;
        private Model _runtimeModel;

        public IWorker worker;
        public Prediction prediction;



        public void SetupModel()
        {
            _runtimeModel = ModelLoader.Load(model);
            worker = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.GPU);

            prediction = new Prediction();
        }



        private void OnDestroy()
        {
            worker?.Dispose();
        }
    }
}