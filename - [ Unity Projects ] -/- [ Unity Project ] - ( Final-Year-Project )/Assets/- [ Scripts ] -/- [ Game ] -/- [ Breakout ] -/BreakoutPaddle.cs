using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Component script for the paddle game object within the Breakout mini-game
// Used to bounce the ball into blocks
public class BreakoutPaddle : MonoBehaviour
{
    [SerializeField] private Vector3 _size;
    [SerializeField] private float _movementSpeed;
    private MiniGameController_Breakout _miniGameControllerBreakout;
    private Bounds _gameBounds;
    private float _curve;

    private Transform _transform;
    
    [SerializeField] private InputActionReference _moveLeft;
    [SerializeField] private InputActionReference _moveRight;
    
    private float _yPosition;
    private Hover _hoverScript;
    
    
    private void Awake()
    {
        _transform = transform;
        _hoverScript = GetComponent<Hover>();

        _yPosition = _transform.position.y;
        
        _miniGameControllerBreakout = FindObjectOfType<MiniGameController_Breakout>();
        _gameBounds = _miniGameControllerBreakout.Bounds;
        _curve = _miniGameControllerBreakout.Curve;
    }




    // Checks for inputs and the calls the movement functions
    private void Update()
    {
        if (_moveLeft.action.ReadValue<float>() > 0) MoveLeft();
        if (_moveRight.action.ReadValue<float>() > 0) MoveRight();
    }

    
    
    
    // Moves the paddle left, checks while doing so if the paddle with leave the game bounds.
    private void MoveLeft()
    {
        float xPosition = Mathf.Clamp(_transform.position.x - (Time.deltaTime * _movementSpeed), _gameBounds.min.x, _gameBounds.max.x);

        Vector3 position = new Vector3(xPosition, _yPosition, _gameBounds.center.z * Mathf.Cos(xPosition * _curve));
        Quaternion rotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(xPosition * _curve) * 40.0f, 0));
        
        _transform.SetPositionAndRotation(position, rotation);
        _hoverScript.basePosition = position;
        _hoverScript.baseRotation = rotation;
    }

    // Moves the paddle right, checks while doing so if the paddle with leave the game bounds.
    private void MoveRight()
    {
        float xPosition = Mathf.Clamp(_transform.position.x + (Time.deltaTime * _movementSpeed), _gameBounds.min.x, _gameBounds.max.x);

        Vector3 position = new Vector3(xPosition, _yPosition, _gameBounds.center.z * Mathf.Cos(xPosition * _curve));
        Quaternion rotation = Quaternion.Euler(new Vector3(0, Mathf.Sin(xPosition * _curve) * 40.0f, 0));
        
        _transform.SetPositionAndRotation(position, rotation);
        _hoverScript.basePosition = position;
        _hoverScript.baseRotation = rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(-_size / 2, new Vector3(0.125f,0.125f, 0.125f));
        Gizmos.DrawCube(_size / 2, new Vector3(0.125f,0.125f, 0.125f));
    }
}
