using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Cosmetic class for giving objects within the game world a slight hovering motion.
public class Hover : MonoBehaviour
{
    [HideInInspector] public Vector3 basePosition;
    [HideInInspector] public Quaternion baseRotation;

    private Transform _transform;

    [SerializeField] private bool _effectsRotation;
    [SerializeField] private float _rotationEffectAmount = 1;
    
    [Space]
    [SerializeField] private bool _effectsPosition;
    [SerializeField] private float _positionEffectAmount = 1;
    
    [Space]
    [SerializeField] private float _effectSpeed = 1;

    private float[] _randomisedTimeOffsets;
    private float[] _randomisedPositionOffsets;
    
    
    // Generates a randomised set of offset values to be used later within the script.
    // Also sets the base state of the game object.
    void Start()
    {
        _transform = transform;
        
        basePosition = _transform.position;
        baseRotation = _transform.rotation;

        _randomisedTimeOffsets = new[]
        {
            Random.Range(0.95f, 1.05f),
            Random.Range(0.95f, 1.05f),
            Random.Range(0.95f, 1.05f),
            Random.Range(0.95f, 1.05f)
        };
        
        _randomisedPositionOffsets = new[]
        {
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        };
    }

    // Using the basePosition and baseTransform variables set at start along side an progressive offset in which smoothly changes over time, the position is set for the game object.
    void Update()
    {
        float time = Time.timeSinceLevelLoad;

        Vector3 position = basePosition + new Vector3(0, Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[0])) * _positionEffectAmount * _randomisedPositionOffsets[0], 0);
        Quaternion rotation = baseRotation * Quaternion.Euler(new Vector3(
            Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[1])) * _rotationEffectAmount * _randomisedPositionOffsets[1], 
            Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[2])) * _rotationEffectAmount * _randomisedPositionOffsets[2], 
            Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[3])) * _rotationEffectAmount * _randomisedPositionOffsets[3]));
        
        if (_effectsPosition) _transform.position = position;
        if (_effectsRotation) _transform.rotation = rotation;
    }
}
