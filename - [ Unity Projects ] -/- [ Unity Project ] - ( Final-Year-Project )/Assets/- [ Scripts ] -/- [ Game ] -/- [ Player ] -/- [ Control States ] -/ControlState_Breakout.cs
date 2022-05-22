using System.Collections;
using System.Collections.Generic;
using _Barracuda;
using Unity.Barracuda;
using UnityEngine;

public class ControlState_Breakout : ControlState_Interface
{
    private List<float[]> _activeSample = new List<float[]>();
    
    
    
    public override void OnStateEnter(Player player)
    {
        _waiting = true;
        player.StartCoroutine(InteractionCooldown());
        
        player.focusActivation = 0;
    }

    public override void OnStateUpdate(Player player)
    {
        if (!player.blinking)
        {
            _activeSample = player.lslInputStream_MotorImagery.Samples;
        }

        if (!_waiting)
        {
            if (_activeSample.Count > 0)
            {
                foreach (float[] sampleSet in _activeSample)
                {
                    ProcessFocusState(ref player, sampleSet);
                }
            }
            else
            {
                Debug.LogWarning("There are no samples of focus data");
            }
        }
        
        player.leftActivation = Mathf.Clamp(player.leftActivation - (Player.growthRate / 2.0f), 0, 1);
        player.rightActivation = Mathf.Clamp(player.rightActivation - (Player.growthRate / 2.0f), 0, 1);
    }

    private void ProcessFocusState(ref Player player, float[] sampleSet)
    {
        Barracuda_Model model = player.focusModel;
        
        Tensor inputTensor = new Tensor(1, 1, 1, 14, sampleSet);
        Tensor outputTensor = model.worker.Execute(inputTensor).PeekOutput();

        model.prediction.SetOutput(ref outputTensor);
        
        float focusResult = Mathf.Clamp01(model.prediction.Outputs[0]);
        player.focusActivation = focusResult;
        
        Debug.Log($"Focus: {focusResult}");

        inputTensor.Dispose();
        outputTensor.Dispose();
    }
}
