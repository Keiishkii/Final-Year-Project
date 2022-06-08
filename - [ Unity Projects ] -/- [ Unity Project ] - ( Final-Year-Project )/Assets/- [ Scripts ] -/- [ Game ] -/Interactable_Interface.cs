using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Interface class for interactable objects. 
public abstract class Interactable_Interface : MonoBehaviour
{
    public abstract void Activate();
}
