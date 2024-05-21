using System;
using System.Collections;
using Unity.Cinemachine;
using Pokemon;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCameraBase _vCam;
    [SerializeField] private InputController _input;

    [Header("Settings")]
    [SerializeField, Range(0.5f, 3f)] private float _speed = 1f;

    private bool _isRMBPressed;
    private bool _cameraMovementLock;

    private void OnEnable()
    {
        _input.Look += OnLook;
        _input.EnableMouseControlCamera += OnEnableMouseControlCamera;
        _input.DisableMouseControlCamera += OnDisableMouseControlCamera;
    }

    private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
    {
        if (_cameraMovementLock) return;

        if (isDeviceMouse && !_isRMBPressed) return;

        float deviceSpeed = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime; 

        // _vCam.m_XAxis.m_InputAxisValue = cameraMovement.x * _speed * deviceSpeed;
        // _vCam.m_YAxis.m_InputAxisValue = cameraMovement.y * _speed * deviceSpeed;
    }

    private void OnDisableMouseControlCamera()
    {
        _isRMBPressed = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // _vCam.m_XAxis.m_InputAxisValue = 0f;
        // _vCam.m_YAxis.m_InputAxisValue = 0f;

        StartCoroutine(DisableMouseForFrame());

    }

    private void OnEnableMouseControlCamera()
    {
        _isRMBPressed = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(DisableMouseForFrame());
    }

    private IEnumerator DisableMouseForFrame()
    {
        _cameraMovementLock = true;
        yield return new WaitForEndOfFrame();
    }

    private void OnDisable()
    {
        _input.Look -= OnLook;
        _input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
        _input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
