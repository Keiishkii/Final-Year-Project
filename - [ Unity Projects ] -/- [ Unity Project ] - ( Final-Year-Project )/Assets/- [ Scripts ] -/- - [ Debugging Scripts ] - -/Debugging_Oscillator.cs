using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Moves and object back and forth between a specific position on game start. Used as a way of continuously moving a game object when the position data is streamed over an LSL network. 
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

    // Generates the random offsets used within the equations in update.
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

    // Moves the game objects position each frame based on the outputs of a simple equation. 
    private void SetPosition()
    {
        float timeSinceLevelLoad = _timeScale * Time.timeSinceLevelLoad;
        float sinT = _distanceScale * Mathf.Sin(timeSinceLevelLoad);
        float cosT = _distanceScale * Mathf.Cos(timeSinceLevelLoad);
        
        Vector3 _offset = new Vector3(_xRandom * sinT, _yRandom * cosT, _zRandom * sinT * cosT);
        
        _transform.position = _basePosition + _offset;
    }
}
