using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface class for a player component, acts as the parent of Player and Debug_Player
public abstract class Player_Interface : MonoBehaviour
{
    public const float growthRate = 0.05f;

    protected bool _allowLeftInput, _allowRightInput;
    [HideInInspector] public float focusActivation, leftActivation, rightActivation;
    protected float blinkActivation = 0;
    [HideInInspector] public bool blinking;
}
