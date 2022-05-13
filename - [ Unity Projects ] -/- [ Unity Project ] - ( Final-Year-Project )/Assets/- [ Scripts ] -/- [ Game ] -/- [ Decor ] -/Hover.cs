using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hover : MonoBehaviour
{
    private Vector3 _basePosition;
    private Quaternion _baseRotation;

    private Transform _transform;

    [SerializeField] private float _rotationEffectAmount = 1;
    [SerializeField] private float _positionEffectAmount = 1;
    [SerializeField] private float _effectSpeed = 1;

    private float[] _randomisedTimeOffsets;
    private float[] _randomisedPositionOffsets;
    
    
    
    void Start()
    {
        _transform = transform;
        
        _basePosition = _transform.position;
        _baseRotation = _transform.rotation;

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

    void Update()
    {
        float time = Time.timeSinceLevelLoad;

        Vector3 position = _basePosition + new Vector3(0, Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[0])) * _positionEffectAmount * _randomisedPositionOffsets[0], 0);
        Quaternion rotation = _baseRotation * Quaternion.Euler(new Vector3(
            Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[1])) * _rotationEffectAmount * _randomisedPositionOffsets[1], 
            Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[2])) * _rotationEffectAmount * _randomisedPositionOffsets[2], 
            Mathf.Sin((time * _effectSpeed * _randomisedTimeOffsets[3])) * _rotationEffectAmount * _randomisedPositionOffsets[3]));
        
        _transform.SetPositionAndRotation(position, rotation);
    }
}
