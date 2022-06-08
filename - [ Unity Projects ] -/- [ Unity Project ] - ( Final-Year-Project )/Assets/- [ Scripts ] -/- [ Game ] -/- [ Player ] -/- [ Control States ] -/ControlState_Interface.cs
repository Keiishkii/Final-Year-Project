using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface class for the player states.
public abstract class ControlState_Interface
{
    protected bool _waiting;
    
    public virtual void OnStateEnter(Player player) {}
    public virtual void OnStateUpdate(Player player) {}
    public virtual void OnStateExit(Player player) {}
    
    protected IEnumerator InteractionCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        _waiting = false;
    }
}
