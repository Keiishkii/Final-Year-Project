using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Integration script for the path based particle system package.
// Stores positions for the path.
public class Beam : MonoBehaviour
{
    [SerializeField] private Transform _start;
    public Vector3 Start
    {
        set => _start.position = value;
    }
    
    [SerializeField] private Transform _middle;
    public Vector3 Middle
    {
        set => _middle.position = value;
    }
    
    
    [SerializeField] private Transform _end;
    public Vector3 End
    {
        set => _end.position = value;
    }
}
