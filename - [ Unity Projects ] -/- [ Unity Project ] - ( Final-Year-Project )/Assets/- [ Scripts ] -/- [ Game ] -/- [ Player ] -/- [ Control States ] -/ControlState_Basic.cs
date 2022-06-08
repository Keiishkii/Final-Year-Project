using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The basic control state, used during the main menu and the results screens.
public class ControlState_Basic : ControlState_Interface
{
    // Updates the players arm activations on based on the input of the mouse.
    public override void OnStateUpdate(Player player)
    {
        player.leftActivation = Mathf.Clamp(player.leftActivation + ((player.mouseLeftClickInputAction.action.ReadValue<float>() > 0) ? Player.growthRate : -Player.growthRate), 0, 1);
        player.rightActivation = Mathf.Clamp(player.rightActivation + ((player.mouseRightClickInputAction.action.ReadValue<float>() > 0) ? Player.growthRate : -Player.growthRate), 0, 1);
    }
}
