using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CrystalSequence;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Debug_PlayerController : Player_Interface
{
    private const float _maxHeightOffset = 1.35f;
    
    [SerializeField] private GameObject _debugCanvasPrefab;
    private GameObject _debugCanvas;
    private Debug_Canvas _debugCanvasScript;
    
    [SerializeField] private InputActionReference _mousePositionInputAction;
    [SerializeField] private InputActionReference _mouseLeftClickInputAction;
    [SerializeField] private InputActionReference _mouseRightClickInputAction;

    [SerializeField] private float _mouseSpeed = 1.0f;
    [SerializeField] private LayerMask _interactableLayerMask;

    [SerializeField] private Transform _avatarTransform;
    [SerializeField] private TwoBoneIKConstraint _leftArmIKConstraint;
    [SerializeField] private TwoBoneIKConstraint _rightArmIKConstraint;
    private Quaternion _desiredAvatarRotation;
    
    private Transform _transform;
    private Vector2 _offsets = Vector2.zero;

    [SerializeField] private Vector3 _armIKOffset;
    private Transform _rightArmIKTarget;
    private Transform _leftArmIKTarget;
    
    
    
    
    private void Awake()
    {
        _transform = transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _mousePositionInputAction.action.started += OnMouseDeltaChange;
        
        _debugCanvas = Instantiate(_debugCanvasPrefab);
        _debugCanvasScript = _debugCanvas.GetComponent<Debug_Canvas>();

        _rightArmIKTarget = _rightArmIKConstraint.data.target.transform;
        _leftArmIKTarget = _leftArmIKConstraint.data.target.transform;
    }

    private void OnDestroy()
    {
        _mousePositionInputAction.action.started -= OnMouseDeltaChange;
    }

    private void OnMouseDeltaChange(InputAction.CallbackContext value)
    {
        Vector2 mouseInput = value.ReadValue<Vector2>();
        if (Mathf.Abs(mouseInput.sqrMagnitude) > 0)
        {
            mouseInput *= _mouseSpeed;
            _offsets += mouseInput;
            _offsets.y = Mathf.Clamp(_offsets.y, -_maxHeightOffset, _maxHeightOffset);

            Vector3 lookDirection = new Vector3(
                Mathf.Sin(_offsets.x) * Mathf.Cos(_offsets.y),
                Mathf.Sin(_offsets.y),
                Mathf.Cos(_offsets.x) * Mathf.Cos(_offsets.y)
            );

            _transform.rotation = Quaternion.LookRotation(Vector3.Normalize(lookDirection));
            
            Vector3 avatarDirection = new Vector3(
                Mathf.Sin(_offsets.x),
                0,
                Mathf.Cos(_offsets.x)
            );

            _desiredAvatarRotation = Quaternion.LookRotation(Vector3.Normalize(avatarDirection));
        }
    }

    private void Update()
    {
        _avatarTransform.rotation = Quaternion.Lerp(_avatarTransform.rotation, _desiredAvatarRotation, 0.025f);

        rightActivation = Mathf.Clamp(rightActivation + ((_mouseRightClickInputAction.action.ReadValue<float>() > 0) ? growthRate : -growthRate), 0, 1);
        _rightArmIKConstraint.weight = Mathf.Lerp(0.4f, 1f, rightActivation);
        
        _debugCanvasScript.SetRightActivation(rightActivation);
        
        leftActivation = Mathf.Clamp(leftActivation + ((_mouseLeftClickInputAction.action.ReadValue<float>() > 0) ? growthRate : -growthRate), 0, 1);
        _leftArmIKConstraint.weight = Mathf.Lerp(0.4f, 1f, leftActivation);
        
        _debugCanvasScript.SetLeftActivation(leftActivation);

        
        
        Vector3 position = _transform.position, cameraForward = _transform.forward, cameraRight = _transform.right, cameraUp = _transform.up;
        Vector3 rightHandTarget = position + (cameraRight * -_armIKOffset.x) + (cameraUp * _armIKOffset.y) + (cameraForward * _armIKOffset.z);
        Vector3 leftHandTarget = position + (cameraRight * _armIKOffset.x) + (cameraUp * _armIKOffset.y) + (cameraForward * _armIKOffset.z);

        _rightArmIKTarget.position = rightHandTarget;
        _leftArmIKTarget.position = leftHandTarget;
        
        

        if (_allowLeftInput && leftActivation > 0.9f)
        {
            _allowLeftInput = false;
            Ray ray = new Ray(_transform.position, _transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, _interactableLayerMask))
            {
                Interactable_Interface interactable = hit.collider.GetComponent<Interactable_Interface>();
                if (interactable)
                {
                    interactable.Activate();
                }
            }
        }
        else if (!_allowLeftInput && leftActivation < 0.3f)
        {
            _allowLeftInput = true;
        }
        
        if (_allowRightInput && rightActivation > 0.9f)
        {
            _allowRightInput = false;
            Ray ray = new Ray(_transform.position, _transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 10.0f, _interactableLayerMask))
            {
                Interactable_Interface interactable = hit.collider.GetComponent<Interactable_Interface>();
                if (interactable)
                {
                    interactable.Activate();
                }
            }
        }
        else if (!_allowRightInput && rightActivation < 0.3f)
        {
            _allowRightInput = true;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Vector3 forward = transform.forward;

        //Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 right = transform.right;
        //Vector3 up = Vector3.Cross(forward, right);
        Vector3 up = transform.up;

        Vector3 xLine = transform.position + (right * _armIKOffset.x);
        Vector3 yLine = xLine + (up * _armIKOffset.y);
        Vector3 zLine = yLine + (forward * _armIKOffset.z);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(position, xLine);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(yLine, xLine);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(yLine, zLine);
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(zLine, 0.125f);
    }
}
