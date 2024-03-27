using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPick : MonoBehaviour
{
    [SerializeField] private GameObject _jointHolder;
    
    //Dragable Object
    private GameObject _currentObject;
    private Transform _currentObjLeft; //Left Object Point
    private Transform _currentObjRight; //Right Object Point

    //Connect Point
    private FixedJoint _joint;

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
        }
    }

    public void GrabOrPut(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            if (_isHandEmpty && _currentObject != null)
            {
                Debug.Log("Grab");
                _isHandEmpty = false;

                _ikcontrol.SetIKActive(true);

                _joint = _jointHolder.AddComponent<FixedJoint>();
                _joint.connectedBody = _currentObject.GetComponent<Rigidbody>();

                _currentObjLeft = _currentObject.GetComponent<Dragable>().PointLeft;
                _currentObjRight = _currentObject.GetComponent<Dragable>().PointRight;

                gameObject.GetComponent<IKControl>().HandObjLeft = _currentObjLeft;
                gameObject.GetComponent<IKControl>().HandObjRight = _currentObjRight;
            }
            else if(!_isHandEmpty && _currentObject)
            {
                Debug.Log("Put");
                _isHandEmpty = true;

                _currentObjLeft = null;
                _currentObjRight = null;

                Destroy(_jointHolder.GetComponent<FixedJoint>());

                _ikcontrol.SetIKActive(false);

                gameObject.GetComponent<IKControl>().HandObjLeft = null;
                gameObject.GetComponent<IKControl>().HandObjRight = null;
            }
            else
            {
                Debug.Log("Empty");
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
