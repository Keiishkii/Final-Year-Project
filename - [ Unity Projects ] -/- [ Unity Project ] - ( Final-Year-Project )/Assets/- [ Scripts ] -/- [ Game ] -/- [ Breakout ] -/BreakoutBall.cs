using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public class BreakoutBall : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private AudioClip _impactWithWall;
    [SerializeField] private AudioClip _impactWithBlock;
    private AudioSource _audioSource;
    
    [Space]
    [SerializeField] private float _autoTargetCooldown;
    
    [Space]
    [SerializeField] private float _minMovementSpeed;
    [SerializeField] private float _maxMovementSpeed;
    private float _movementSpeed;
    
    private Vector2 _velocity = Vector2.zero;
    [SerializeField] private float _rotationSpeed;
    private Quaternion _rotation = Quaternion.identity;

    private IEnumerator _targetCoroutine;

    private Player _player;
    
    private MiniGameController_Breakout _miniGameControllerBreakout;
    private Bounds _gameBounds;
    private float _curve;
    
    
    
    
    private void Awake()
    {
        _transform = transform;
        _audioSource = GetComponent<AudioSource>();
        
        _player = FindObjectOfType<Player>();
        
        _miniGameControllerBreakout = FindObjectOfType<MiniGameController_Breakout>();
        _gameBounds = _miniGameControllerBreakout.Bounds;
        _curve = _miniGameControllerBreakout.Curve;
    }
    
    void Start()
    {
        StartCoroutine(Begin());
    }

    private IEnumerator Begin()
    {
        yield return new WaitForSeconds(1.5f);

        float x = Random.Range(-2.0f, 2.0f);

        _velocity = new Vector2(x, 1);
        _velocity.Normalize();

        ResetBallTargetingCountdown();
    }

    private IEnumerator TargetBlock()
    {
        yield return new WaitForSeconds(_autoTargetCooldown);

        Vector3 position = _transform.position;
        
        GameObject nearestBlock = _miniGameControllerBreakout.GetNearestBlock(_transform.position);
        Vector3 positionOfOther = nearestBlock.transform.position;
        
        _velocity =  positionOfOther - position;
        _velocity.Normalize();
    }
    
    
    
    private void FixedUpdate()
    {
        float targetSpeed = Mathf.Lerp(_minMovementSpeed, _maxMovementSpeed, _player.focusActivation);
        _movementSpeed = Mathf.Lerp(_movementSpeed, targetSpeed, 0.025f);
        
        Vector3 position = _transform.position;
        Quaternion rotation = _transform.rotation;
        
        BoundsCheck(ref position);
        UpdateBallTransform(ref position, ref rotation);
    }

    private void BoundsCheck(ref Vector3 position)
    {
        if (_gameBounds.max.y <= position.y)
        {
            _velocity.y = -Mathf.Abs(_velocity.y);
            _audioSource.PlayOneShot(_impactWithWall);
        }
        else if (_gameBounds.min.y >= position.y)
        {
            _miniGameControllerBreakout.EndMiniGame(false);
        }
        
        if (_gameBounds.max.x <= position.x)
        {
            _velocity.x = -Mathf.Abs(_velocity.x);
            _audioSource.PlayOneShot(_impactWithWall);
        }
        else if (_gameBounds.min.x >= position.x)
        {
            _velocity.x = Mathf.Abs(_velocity.x);
            _audioSource.PlayOneShot(_impactWithWall);
        }
    }
    
    private void UpdateBallTransform(ref Vector3 position, ref Quaternion rotation)
    {
        Vector2 delta = _velocity * (Time.fixedDeltaTime * _movementSpeed);
        Vector2 twoDimensionalPosition = new Vector2(position.x + delta.x, position.y + delta.y);
        
        position = new Vector3(twoDimensionalPosition.x, twoDimensionalPosition.y, _gameBounds.center.z * Mathf.Cos(twoDimensionalPosition.x * _curve));
        rotation = rotation * _rotation;

        _transform.SetPositionAndRotation(position, rotation);
    }
    
    
    
    private void OnTriggerStay(Collider other)
    {
        switch (other.tag)
        {
            case "Breakout: Block":
            {
                OnBallCollision(other.transform);
                ResetBallTargetingCountdown();
                
                _miniGameControllerBreakout.DestroyBlock(other.gameObject.GetInstanceID());
            } break;
            case "Breakout: Paddle":
            {
                OnBallCollision(other.transform);
                ResetBallTargetingCountdown();
            } break;
            case "Breakout: Emitter":
            {
                OnBallCollision(other.transform);
            } break;
        }
    }

    private void ResetBallTargetingCountdown()
    {
        if (_targetCoroutine != null) StopCoroutine(_targetCoroutine);
        
        _targetCoroutine = TargetBlock();
        StartCoroutine(_targetCoroutine);
    }
    
    private void OnBallCollision(Transform otherTransform)
    {
        Vector2 position = _transform.position;
        Vector2 positionOfOther = otherTransform.position;
        
        _velocity = position - positionOfOther;
        _velocity.Normalize();
        
        _rotation = Quaternion.Euler(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * _rotationSpeed);
        _audioSource.PlayOneShot(_impactWithBlock);
    }
}
