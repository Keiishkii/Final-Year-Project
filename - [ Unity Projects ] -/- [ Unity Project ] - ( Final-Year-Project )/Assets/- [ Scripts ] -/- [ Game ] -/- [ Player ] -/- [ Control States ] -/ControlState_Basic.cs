using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlState_Basic : ControlState_Interface
{
    public override void OnStateUpdate(Player player)
    {
        player.leftActivation = Mathf.Clamp(player.leftActivation + ((player.mouseLeftClickInputAction.action.ReadValue<float>() > 0) ? Player.growthRate : -Player.growthRate), 0, 1);
        player.rightActivation = Mathf.Clamp(player.rightActivation + ((player.mouseRightClickInputAction.action.ReadValue<float>() > 0) ? Player.growthRate : -Player.growthRate), 0, 1);
    }
}
