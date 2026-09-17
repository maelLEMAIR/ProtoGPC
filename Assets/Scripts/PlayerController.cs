using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    
    private Vector2 moveInput;

    public PlayerAttack PlayerAttack;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        //if (_inputManager.PlayerSprint())
        //    _controller.Move(move * (playerSprintSpeed * Time.deltaTime));
        //else

        _controller.Move(move * (playerSpeed * Time.deltaTime));

        _controller.Move(_playerVelocity * Time.deltaTime);

        _playerVelocity.y += gravityValue * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _groundedPlayer = _controller.isGrounded;

        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        if (_groundedPlayer && context.performed)
        {
            _playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerAttack.enabled = true;
            PlayerAttack.Attack();
        }
    }
}
