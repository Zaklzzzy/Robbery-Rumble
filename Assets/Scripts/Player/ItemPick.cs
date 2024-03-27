using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPick : MonoBehaviour
{
    private Transform currentObjLeft; //Left Object Point
    private Transform currentObjRight; //Right Object Point

    private IKControl _ikcontrol;

    private bool _isHandEmpty = true;

    [SerializeField] private GameObject _jointHolder;

    [SerializeField] private FixedJoint _joint;

    [SerializeField] private GameObject _currentObject;

    private void Start()
    {
        _ikcontrol = GetComponent<IKControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable"))
        {
            _currentObject = other.gameObject;
            _joint.connectedBody = other.GetComponent<Rigidbody>();

            currentObjLeft = other.gameObject.GetComponent<Dragable>().PointLeft;
            currentObjRight = other.gameObject.GetComponent<Dragable>().PointRight;
        }
    }

    public void GrabOrPut(InputAction.CallbackContext context)
    {
        if (context.performed)
        { 
            if (_isHandEmpty)
            {
                Debug.Log("Grab");
                _isHandEmpty = false;

                _ikcontrol.SetIKActive(true);

                _joint.connectedBody = _currentObject.GetComponent<Rigidbody>();

                currentObjLeft = _currentObject.GetComponent<Dragable>().PointLeft;
                currentObjRight = _currentObject.GetComponent<Dragable>().PointRight;

                gameObject.GetComponent<IKControl>().HandObjLeft = currentObjLeft;
                gameObject.GetComponent<IKControl>().HandObjRight = currentObjRight;
            }
            else
            {
                Debug.Log("Put");
                _isHandEmpty = true;

                currentObjLeft = null;
                currentObjRight = null;

                Destroy(_jointHolder.GetComponent<FixedJoint>());

                _ikcontrol.SetIKActive(false);

                gameObject.GetComponent<IKControl>().HandObjLeft = null;
                gameObject.GetComponent<IKControl>().HandObjRight = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable"))
        {
            _currentObject = null;
        }
    }
}
