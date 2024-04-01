using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ItemPick : MonoBehaviour
{
    [SerializeField] private GameObject _jointHolder;
    [SerializeField] private GameObject _grabUI;
    [SerializeField] private GameObject _putUI;
    
    //Dragable Object
    private GameObject _currentObject;
    private Transform _currentObjLeft; //Left Object Point
    private Transform _currentObjRight; //Right Object Point

    //Connect Point
    private SpringJoint _joint;

    //For IK Animation
    private IKControl _ikcontrol;

    //Boolean For Grab And Put
    private bool _isHandEmpty = true;

    private void Start()
    {
        _ikcontrol = GetComponent<IKControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable"))
        {
            _currentObject = other.gameObject;
            _grabUI.SetActive(_isHandEmpty);
            //_putUI.SetActive(!_isHandEmpty);
        }
    }

    public void GrabOrPut(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isHandEmpty = !_isHandEmpty;

            if (!_isHandEmpty && _currentObject != null)
            {
                Debug.Log("Grab");
                SetupJointAndIKPoints();
            }
            else
            {
                Debug.Log(_currentObject ? "Put" : "Empty");
                ReleaseObject();
            }

            _grabUI.SetActive(!_isHandEmpty);
            _putUI.SetActive(_isHandEmpty);
        }
    }

    private void SetupJointAndIKPoints()
    {
        _joint = _jointHolder.AddComponent<SpringJoint>();
        _joint.connectedBody = _currentObject.GetComponent<Rigidbody>();

        _joint.spring = 50;
        _joint.damper = 5;
        _joint.breakForce = float.PositiveInfinity;

        var dragable = _currentObject.GetComponent<Dragable>();
        _currentObjLeft = dragable.PointLeft;
        _currentObjRight = dragable.PointRight;

        _ikcontrol.SetIKTargets(_currentObjLeft, _currentObjRight, true);
    }

    private void ReleaseObject()
    {
        if (_joint) Destroy(_joint);
        _ikcontrol.SetIKTargets(null, null, false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable"))
        {
            _currentObject = null;
            _grabUI.SetActive(false);
            _putUI.SetActive(false);
        }
    }
}
