using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private ConfigurableJoint _cj;

    //New Input System
    private GameInput _gameInput;
    //private IControllable _controllable; 

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _gameInput = new GameInput();
        _gameInput.Enable();

        //_controllable = GetComponent<IControllable>();
    }

    private void FixedUpdate()
    {
        var inputDirection = _gameInput.Gameplay.Movement.ReadValue<Vector2>();

        float x = -inputDirection.x;
        float z = inputDirection.y;
        Vector3 move = new Vector3(x, 0, z).normalized;

        if (move != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(move);
            _cj.targetRotation = newRotation;
        }

        _animator.SetFloat("Horizontal", (x+z!=0 ? Math.Abs(x+z) : Math.Abs(-x+z) ));
        //_animator.SetFloat("Vertical", x);
    }

}
