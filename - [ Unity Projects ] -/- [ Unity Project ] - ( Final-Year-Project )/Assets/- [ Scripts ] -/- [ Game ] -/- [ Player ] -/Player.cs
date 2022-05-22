using System;
using System.Collections;
using System.Collections.Generic;
using _Barracuda;
using _LSL;
using Unity.Barracuda;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class Player : Player_Interface
{
    #region Interactions
        [SerializeField] private InputActionReference _mousePositionInputAction;
        public InputActionReference mouseLeftClickInputAction;
        public InputActionReference mouseRightClickInputAction;
    #endregion
    
    [Space]
    #region NeuralNetworks
        public Barracuda_Model leftHandControlModel;
        public Barracuda_Model rightHandControlModel;
        public Barracuda_Model focusModel;
        public Barracuda_Model blinkingModel;
        
        public LSLInputStream_FloatArray lslInputStream_RelaxationAndConcentrationEEG;
        public LSLInputStream_FloatArray lslInputStream_MotorImagery;
        public LSLInputStream_FloatArray lslInputStream_FacialEMG;
    #endregion
    
    [Space]
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private float _mouseSpeed = 1.0f;

    [SerializeField] private GameObject _canvasPrefab;
    private GameObject _canvas;
    
    private ControlState_Interface _activeControlState;
    public ControlState_Interface SetControlState
    {
        set
        {
            _activeControlState?.OnStateExit(this);
            _activeControlState = value;
            _activeControlState.OnStateEnter(this);
        }
    }
    
    public readonly ControlState_Basic ControlStateBasic = new ControlState_Basic();
    public readonly ControlState_CrystalSequence ControlStateCrystalSequence = new ControlState_CrystalSequence();
    public readonly ControlState_Breakout ControlStateBreakout = new ControlState_Breakout();
    
    private Transform _transform;

    #region CameraRotation
        private const float _maxHeightOffset = 1.35f;
    
        private Vector2 _offsets = Vector2.zero;
        private Quaternion _desiredAvatarRotation;
    #endregion
 
    [Space]
    #region Avatar
        [SerializeField] private Transform _avatarTransform;
        [SerializeField] private TwoBoneIKConstraint _leftArmIKConstraint;
        [SerializeField] private TwoBoneIKConstraint _rightArmIKConstraint;
        
        [SerializeField] private Vector3 _armIKOffset;
        [SerializeField] private Vector3 _rightHandIKRotationOffset;
        [SerializeField] private Vector3 _leftHandIKRotationOffset;
        private Transform _rightArmIKTarget;
        private Transform _leftArmIKTarget;

        private float _targetLeftIKWeight;
        private float _targetRightIKWeight;
    #endregion
    
    
    
    
    
    private void Awake()
    {
        _transform = transform;
        SetControlState = ControlStateBasic;
        
        _rightArmIKTarget = _rightArmIKConstraint.data.target.transform;
        _leftArmIKTarget = _leftArmIKConstraint.data.target.transform;
        
        _canvas = Instantiate(_canvasPrefab);
        
        LockMouseToCentreScreen();
        SubscribeToInputEvents();
        SetupNeuralNetworkModels();
    }

    private void OnDestroy()
    {
        UnsubscribeToInputEvents();
    }
    
    
    
    
    
    private void Update()
    {
        ProcessPlayerBlinkState();
        _activeControlState.OnStateUpdate(this);
        
        UpdateAvatar();
        InteractionTest();
    }





    private void SetupNeuralNetworkModels()
    {
        leftHandControlModel.SetupModel();
        rightHandControlModel.SetupModel();
        focusModel.SetupModel();
        
        blinkingModel.SetupModel();
    }
    
    private void SubscribeToInputEvents()
    {
        _mousePositionInputAction.action.started += OnMouseDeltaUpdate;
    }
    
    private void UnsubscribeToInputEvents()
    {
        _mousePositionInputAction.action.started -= OnMouseDeltaUpdate;
    }

    private void LockMouseToCentreScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnMouseDeltaUpdate(InputAction.CallbackContext value)
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

    private void UpdateAvatar()
    {
        _avatarTransform.rotation = Quaternion.Lerp(_avatarTransform.rotation, _desiredAvatarRotation, 0.05f);

        _targetRightIKWeight = Mathf.Lerp(0.4f, 1f, Mathf.InverseLerp(0.25f, 1f, rightActivation));
        _rightArmIKConstraint.weight = Mathf.Lerp(_rightArmIKConstraint.weight, _targetRightIKWeight, 0.125f);
        
        _targetLeftIKWeight = Mathf.Lerp(0.4f, 1f, Mathf.InverseLerp(0.25f, 1f, leftActivation));
        _leftArmIKConstraint.weight = Mathf.Lerp(_leftArmIKConstraint.weight, _targetLeftIKWeight, 0.125f);
        
        
        Vector3 position = _transform.position, cameraForward = _transform.forward, cameraRight = _transform.right, cameraUp = _transform.up;
        Vector3 rightHandTarget = position + (cameraRight * -_armIKOffset.x) + (cameraUp * _armIKOffset.y) + (cameraForward * _armIKOffset.z);
        Vector3 leftHandTarget = position + (cameraRight * _armIKOffset.x) + (cameraUp * _armIKOffset.y) + (cameraForward * _armIKOffset.z);

        _rightArmIKTarget.position = rightHandTarget;
        _rightArmIKTarget.rotation = _transform.rotation * quaternion.Euler(_rightHandIKRotationOffset * Mathf.Deg2Rad);
        
        _leftArmIKTarget.position = leftHandTarget;
        _leftArmIKTarget.rotation = _transform.rotation * quaternion.Euler(_leftHandIKRotationOffset * Mathf.Deg2Rad);
    }

    private void InteractionTest()
    {
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

    private float blinkActivation = 0;
    private void ProcessPlayerBlinkState()
    {
        List<float[]> samples = lslInputStream_FacialEMG.Samples;
        if (samples.Count > 0)
        {
            foreach (float[] sampleSet in samples)
            {
                Barracuda_Model model = blinkingModel;
        
                Tensor inputTensor = new Tensor(1, 1, 1, 2, sampleSet);
                Tensor outputTensor = model.worker.Execute(inputTensor).PeekOutput();

                model.prediction.SetOutput(ref outputTensor);

                float result = Mathf.Clamp01(model.prediction.Outputs[0]);
                blinkActivation = Mathf.Lerp(blinkActivation, result, 0.125f);
                blinking = (blinkActivation < 0.5f);
                
                inputTensor.Dispose();
                outputTensor.Dispose();
            }
        }
    }
}
