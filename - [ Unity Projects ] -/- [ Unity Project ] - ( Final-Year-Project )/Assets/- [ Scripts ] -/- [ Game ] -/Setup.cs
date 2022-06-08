using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Setup component, used to set project settings on game load.
public class Setup : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
