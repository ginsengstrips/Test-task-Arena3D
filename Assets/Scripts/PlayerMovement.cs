using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _jumpHeight = 2.0f;
    private float _gravity = -9.81f;

    private Vector3 _velocity;
    private bool _isGrounded;

    private PlayerInput _playerInput;
    private InputActionReference _moveActionReference;

    [SerializeField] private CharacterController _controller;

    [Header("Camera Tilt Settings")]
    [SerializeField] private float _tiltAmount = 15f;
    [SerializeField] private float _tiltSmoothness = 5f;

    private float _currentTilt = 0f;
    private float _targetTilt = 0f;
    private Quaternion _targetLookRotation = Quaternion.identity;

    private float _xRotation;

    private void Awake()
    {
        _playerInput = GameInputManager.instance.PlayerInput;
    }
    private void OnEnable()
    {
        _playerInput.Player.Jump.performed += OnJumpPerfromed;
    }
    private void OnDisable()
    {
        _playerInput.Player.Jump.performed -= OnJumpPerfromed;
    }
    private void Update()
    {
        Movement();
        LookRotate();
        CameraTilt();
    }
    private void Movement()
    {
        Vector2 playerInput = _playerInput.Player.Movement.ReadValue<Vector2>();
        Vector3 direction = new Vector3(playerInput.x, 0, playerInput.y);
        float cameraYRotation = Camera.main.transform.eulerAngles.y;
        direction = Quaternion.Euler(0, cameraYRotation, 0) * direction;
        if (direction.magnitude > 1f)
            direction.Normalize();
        if (_controller.isGrounded)
        {
            if (_velocity.y < 0)
            {
                _velocity.y = -2f;
            }
        }
        else
        {
            _velocity.y += _gravity * Time.deltaTime;
        }
        Vector3 horizontalMovement = direction * _movementSpeed * Time.deltaTime;
        Vector3 verticalMovement = Vector3.up * _velocity.y * Time.deltaTime;
        _controller.Move(horizontalMovement + verticalMovement);

        _targetTilt = -playerInput.x * _tiltAmount;
        _currentTilt = Mathf.Lerp(_currentTilt, _targetTilt, _tiltSmoothness * Time.deltaTime);
    }

    private void LookRotate()
    {
        Vector2 mouseDelta = _playerInput.Player.Look.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * _mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * _mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _targetLookRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }

    private void CameraTilt()
    {
        Quaternion finalRotation = _targetLookRotation * Quaternion.Euler(0f, 0f, _currentTilt);
        Camera.main.transform.localRotation = Quaternion.Slerp(
            Camera.main.transform.localRotation,
            finalRotation,
            _tiltSmoothness * Time.deltaTime
        );
    }
    private void OnJumpPerfromed(InputAction.CallbackContext obj)
    {
        if (_controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * 2f * -_gravity);
        }
    }
}
