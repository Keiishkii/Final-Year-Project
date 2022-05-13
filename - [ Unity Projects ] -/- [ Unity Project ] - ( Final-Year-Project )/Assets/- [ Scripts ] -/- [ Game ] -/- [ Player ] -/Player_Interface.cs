using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player_Interface : MonoBehaviour
{
    protected const float _growthRate = 0.1f;
    protected bool _allowLeftInput, _allowRightInput;
    protected float _focusActivation, _leftActivation, _rightActivation;
}
