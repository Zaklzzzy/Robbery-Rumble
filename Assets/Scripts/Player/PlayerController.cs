/*using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 15f;
    [SerializeField] private float _turnSpeed = 10f;
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;

    [Header("Jump")]
    [SerializeField] private float _jumpPower = 5f;
    private bool _isGrounded = false;

    //New Input System
    private GameInput _gameInput;
    //private IControllable _controllable;

    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _mainCamera = Camera.main;

        _gameInput = new GameInput();
        _gameInput.Enable();

        //_controllable = GetComponent<IControllable>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        Vector2 inputDirection = _gameInput.Gameplay.Movement.ReadValue<Vector2>();

        float x = inputDirection.x;
        float z = inputDirection.y;

        Vector2 move = new Vector2(x, z);

        //Rotate
        if (move != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, targetRotation, _turnSpeed * Time.deltaTime));
        }

        Vector3 moveDirection = Quaternion.Euler(0f, _rb.rotation.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 velocity = _walkSpeed * moveDirection.normalized * inputDirection.magnitude;

        //Movement
        Vector3 velocityChange = (velocity - new Vector3(_rb.velocity.x, 0, _rb.velocity.z));
        _rb.AddForce(velocityChange, ForceMode.VelocityChange);

        //Animate
        _animator.SetFloat("Horizontal", x);
        _animator.SetFloat("Vertical", z);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded && context.performed)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void CheckGround()
    {
        _isGrounded = Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }

}*/


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 15f;
    [SerializeField] private float _turnSpeed = 10f;
    [SerializeField, Range(0f,1f)] private float _velocityMultiply = 0.95f;
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;

    [Header("Jump")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckDistance = 0.2f;
    [SerializeField] private float _gravityMultiplier = 2.5f;
    public bool _isGrounded = false;

    //New Input System
    private GameInput _gameInput;
    //private IControllable _controllable;

    private Camera _mainCamera;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _mainCamera = Camera.main;

        _gameInput = new GameInput();
        _gameInput.Enable();

        //_controllable = GetComponent<IControllable>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void FixedUpdate()
    {
        //Input
        Vector2 inputDirection = _gameInput.Gameplay.Movement.ReadValue<Vector2>();

        float x = inputDirection.x;
        float z = inputDirection.y;

        if (inputDirection != Vector2.zero)
        {
            //Rotate
            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            _rb.MoveRotation(Quaternion.Lerp(_rb.rotation, targetRotation, _turnSpeed * Time.deltaTime));

            //Movement
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _rb.velocity = new Vector3(moveDirection.x * _walkSpeed, _rb.velocity.y, moveDirection.z * _walkSpeed);
        }
        else
        {
            //Stop
            _rb.velocity = new Vector3(_rb.velocity.x * _velocityMultiply, _rb.velocity.y, _rb.velocity.z * _velocityMultiply);
        }

        if (!_isGrounded && _rb.velocity.y < 0)
        {
            Vector3 extraGravityForce = (Physics.gravity * _gravityMultiplier) - Physics.gravity;
            _rb.AddForce(extraGravityForce);
        }

        //Animate
        _animator.SetFloat("Horizontal", x);
        _animator.SetFloat("Vertical", z);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded && context.performed)
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void CheckGround()
    {
        Vector3 origin = _groundCheck.position;
        _isGrounded = Physics.Raycast(origin, Vector3.down, _groundCheckDistance, _groundLayer);
        //Debug.DrawRay(origin, Vector3.down * groundCheckDistance, Color.red);
    }

}