using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform _yawTransform;
    [SerializeField] private Transform _pitchTransform;
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private float _sensitivity = .1f;
    
    private float _yaw;
    private float _pitch;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        _yaw += _lookAction.action.ReadValue<Vector2>().x * _sensitivity;
        _pitch -= _lookAction.action.ReadValue<Vector2>().y * _sensitivity;
        
        _pitch = Mathf.Clamp(_pitch, -90f, 90f);

        _yawTransform.localRotation = Quaternion.Euler(0f, _yaw, 0f);
        _pitchTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void OnEnable()
    {
        _lookAction.action.Enable();
    }

    private void OnDisable()
    {
        _lookAction.action.Disable();
    }
}
