using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Debugging_Oscillator : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _basePosition;

    private float _xRandom, _yRandom, _zRandom;
    
    [SerializeField] [Min(0.01f)] private float _timeScale;
    [SerializeField] [Min(0.01f)] private float _distanceScale;



    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        _basePosition = _transform.position;

        _xRandom = Random.Range(0.75f, 1f);
        _yRandom = Random.Range(0.75f, 1f);
        _zRandom = Random.Range(0.75f, 1f);

        SetPosition();
    }

    
    
    void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        float timeSinceLevelLoad = _timeScale * Time.timeSinceLevelLoad;
        float sinT = _distanceScale * Mathf.Sin(timeSinceLevelLoad);
        float cosT = _distanceScale * Mathf.Cos(timeSinceLevelLoad);
        
        Vector3 _offset = new Vector3(_xRandom * sinT, _yRandom * cosT, _zRandom * sinT * cosT);
        
        _transform.position = _basePosition + _offset;
    }
}
