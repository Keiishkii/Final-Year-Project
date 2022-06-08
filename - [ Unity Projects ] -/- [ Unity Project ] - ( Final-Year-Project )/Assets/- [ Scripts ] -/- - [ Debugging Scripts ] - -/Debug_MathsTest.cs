using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A debug script for testing the reflection angle of a traveling object against a plane
public class Debug_MathsTest : MonoBehaviour
{
    [SerializeField] private Transform _reflectorTransform;
    [SerializeField] private Vector3 _velocity;

    // Renders lines to visually demonstrate the maths being applied.  
    private void OnDrawGizmos()
    {
        if (_reflectorTransform)
        {
            Vector3 position = transform.position;
            Vector3 positionOfReflector = _reflectorTransform.position;
            
            Gizmos.DrawLine(position - _velocity, position);
            Gizmos.DrawWireSphere(positionOfReflector, Vector3.Distance(position, positionOfReflector));
            
            Gizmos.DrawLine(positionOfReflector, position + (position - positionOfReflector));
            
            Vector3 reflectedVelocity = Vector3.Reflect(_velocity, Vector3.Normalize(position - positionOfReflector));

            if (Vector3.Angle(reflectedVelocity, (position - positionOfReflector)) > 90)
            {
                Gizmos.DrawLine(position, position - reflectedVelocity);
            }
            else
            {
                Gizmos.DrawLine(position, position + reflectedVelocity);
            }
        }
    }
}
