using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class PhisicalBodyPart : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private ConfigurableJoint _joint;
    private Quaternion _startRotation;
    private void Start()
    {
        _joint = GetComponent<ConfigurableJoint>();
        _startRotation = transform.localRotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _joint.targetRotation = Quaternion.Inverse(_target.localRotation) * _startRotation;
    }
}
