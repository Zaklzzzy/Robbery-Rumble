using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPick : NetworkBehaviour
{
    [SerializeField] private GameObject _jointHolder;
    //[SerializeField] private GameObject _grabUI;
    //[SerializeField] private GameObject _putUI;
    
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
            /*if (_isHandEmpty)
            {
                _grabUI.SetActive(true);
                _putUI.SetActive(false);
            }
            else if (!_isHandEmpty)
            {
                _grabUI.SetActive(false);
                _putUI.SetActive(true);
            }*/
        }
    }

    public void GrabOrPut(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            if (_isHandEmpty && _currentObject != null)
            {
                Debug.Log("Grab");
                //_putUI.SetActive(true);
                //_grabUI.SetActive(false);

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
                //_putUI.SetActive(false);

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
            //_grabUI.SetActive(false);
            //_putUI.SetActive(false);
        }
    }
}
