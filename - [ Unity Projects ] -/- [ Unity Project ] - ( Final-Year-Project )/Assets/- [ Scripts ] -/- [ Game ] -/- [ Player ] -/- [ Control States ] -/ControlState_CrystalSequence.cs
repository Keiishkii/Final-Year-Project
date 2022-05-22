using System.Collections;
using System.Collections.Generic;
using _Barracuda;
using Unity.Barracuda;
using UnityEngine;

public class ControlState_CrystalSequence : ControlState_Interface
{
    private List<float[]> _activeSample = new List<float[]>();
    
    
    
    public override void OnStateEnter(Player player)
    {
        _waiting = true;
        player.StartCoroutine(InteractionCooldown());
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
                    ProcessLeftHandState(ref player, sampleSet);
                    ProcessRightHandState(ref player, sampleSet);
                }
            }
            else
            {
                Debug.LogWarning("There are no samples of motor imagery data");
            }
        }
        else
        {
            player.leftActivation = Mathf.Clamp(player.leftActivation - (Player.growthRate / 2.0f), 0, 1);
            player.rightActivation = Mathf.Clamp(player.rightActivation - (Player.growthRate / 2.0f), 0, 1);
        }
    }

    private void ProcessLeftHandState(ref Player player, float[] sampleSet)
    {
        Barracuda_Model model = player.leftHandControlModel;
        
        Tensor inputTensor = new Tensor(1, 1, 1, 64, sampleSet);
        Tensor outputTensor = model.worker.Execute(inputTensor).PeekOutput();

        model.prediction.SetOutput(ref outputTensor);
        switch (model.prediction.OutputIndex)
        {
            case 0:
            {
                player.leftActivation = Mathf.Clamp(player.leftActivation - (Player.growthRate / 2.0f), 0, 1);
            } break;
            case 1:
            {
                player.leftActivation = Mathf.Clamp(player.leftActivation + Player.growthRate, 0, 1);
            } break;
        }
        
        inputTensor.Dispose();
        outputTensor.Dispose();
    }
    
    private void ProcessRightHandState(ref Player player, float[] sampleSet)
    {
        Barracuda_Model model = player.rightHandControlModel;
        
        Tensor inputTensor = new Tensor(1, 1, 1, 64, sampleSet);
        Tensor outputTensor = model.worker.Execute(inputTensor).PeekOutput();

        model.prediction.SetOutput(ref outputTensor);
        switch (model.prediction.OutputIndex)
        {
            case 0:
            {
                player.rightActivation = Mathf.Clamp(player.rightActivation - (Player.growthRate / 2.0f), 0, 1);
            } break;
            case 1:
            {
                player.rightActivation = Mathf.Clamp(player.rightActivation + Player.growthRate, 0, 1);
            } break;
        }

        inputTensor.Dispose();
        outputTensor.Dispose();
    }
}
