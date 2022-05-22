using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_Interface : MonoBehaviour
{
    public const float growthRate = 0.05f;

    protected bool _allowLeftInput, _allowRightInput;
    [HideInInspector] public float focusActivation, leftActivation, rightActivation;
    [HideInInspector] public bool blinking;
}
