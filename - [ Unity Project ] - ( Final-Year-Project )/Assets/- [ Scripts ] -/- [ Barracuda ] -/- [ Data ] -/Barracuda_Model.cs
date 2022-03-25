using Unity.Barracuda;
using UnityEngine;

public abstract class Barracuda_Model : MonoBehaviour
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