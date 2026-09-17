using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private float _sensitivity;

    public Transform playerBody;
    public Transform playerHead;
    
    
    private Vector2 _mMouseInput;

    private float _xRotation = 0.0f;
    

    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private Vector2 GetMouseDelta()
    {
        if (Mouse.current != null)
        {
            return Mouse.current.delta.ReadValue();
        }
        else
        {
            return Vector2.zero;
        }
    }

    private void Update()
    {
        Vector2 mouseInput =  GetMouseDelta() * _sensitivity;

        _xRotation -= mouseInput.y;
        _xRotation = Mathf.Clamp(_xRotation, -50f, 89f);

        playerHead.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseInput.x);
    }
}
