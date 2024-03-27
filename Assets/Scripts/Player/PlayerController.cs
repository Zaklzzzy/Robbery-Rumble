using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;

    //New Input System
    private GameInput _gameInput;
    //private IControllable _controllable;

    //Rotate And Movement
    
    //RaycastHit _hit;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _gameInput = new GameInput();
        _gameInput.Enable();

        //_controllable = GetComponent<IControllable>();
    }

    private void FixedUpdate()
    {
        var inputDirection = _gameInput.Gameplay.Movement.ReadValue<Vector2>();

        float x = inputDirection.x;
        float z = inputDirection.y;

        Vector3 move = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

        if (move != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(move);

            _rb.rotation = Quaternion.Slerp(_rb.rotation, newRotation, _moveSpeed * Time.deltaTime);
        }

        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out _hit);
        Vector3 targetPosition = new Vector3(_hit.point.x, transform.position.y, _hit.point.z + 2.3f);
        transform.LookAt(targetPosition);*/

        _rb.MovePosition(_rb.position + move * _moveSpeed * Time.deltaTime);

        _animator.SetFloat("Horizontal", x);
        _animator.SetFloat("Vertical", z);
    }

}
