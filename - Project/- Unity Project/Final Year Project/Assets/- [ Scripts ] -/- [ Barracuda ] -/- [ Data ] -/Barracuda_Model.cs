using Unity.Barracuda;
using UnityEngine;

public abstract class Barracuda_Model : MonoBehaviour
{
    [SerializeField] public NNModel model;
    private Model _runtimeModel;
    
    public IWorker worker;
    public Prediction prediction;
    
    
    
    void Awake()
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