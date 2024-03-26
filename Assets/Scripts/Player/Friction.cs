using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
    [SerializeField] PhysicMaterial _defaultFriction;
    [SerializeField] PhysicMaterial _zeroFriction;

    [SerializeField] Collider _leftCollider;
    [SerializeField] Collider _rightCollider;

    public void SetLeftFriction()
    {
        _leftCollider.material = _defaultFriction;
        _rightCollider.material = _zeroFriction;
    }
    public void SetRightFriction()
    {
        _leftCollider.material = _zeroFriction;
        _rightCollider.material = _defaultFriction;
    }
}
