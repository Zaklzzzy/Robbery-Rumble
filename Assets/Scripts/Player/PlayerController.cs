using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _animator;

    //New Input System
    private GameInput _gameInput;
    //private IControllable _controllable;

    //Rotate And Movement
    private Rigidbody _rb;
    //RaycastHit _hit;

    private void Awake()
    {
        _rb = GetComponentInChildren<Rigidbody>();

        _animator = GetComponentInChildren<Animator>();

        _gameInput = new GameInput();
        _gameInput.Enable();

        //_controllable = GetComponent<IControllable>();
    }

    private void FixedUpdate()
    {
        var inputDirection = _gameInput.Gameplay.Movement.ReadValue<Vector2>();

        float x = inputDirection.x;
        float z = inputDirection.y;
        Vector3 move = new Vector3(x, 0, z);


        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out _hit);
        Vector3 targetPosition = new Vector3(_hit.point.x, transform.position.y, _hit.point.z + 2.3f);
        transform.LookAt(targetPosition);*/

        _rb.MovePosition(transform.position + move.normalized * _moveSpeed * Time.deltaTime);
        _animator.SetFloat("Horizontal", x);
        _animator.SetFloat("Vertical", z);
    }

}
