using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPick : NetworkBehaviour
{
    [SerializeField] private GameObject _jointHolder;
    [SerializeField] private GameObject _KeyUI;
    [SerializeField] private TextMeshProUGUI _KeyText;
    
    //Dragable Object
    public GameObject _currentObject;
    private Transform _currentObjLeft; //Left Object Point
    private Transform _currentObjRight; //Right Object Point

    //Connect Point
    private Rigidbody _rb;

    //For IK Animation
    private IKControl _ikcontrol;

    //Boolean For Grab And Put
    private bool _isHandEmpty = true;

    private void Start()
    {
        _ikcontrol = GetComponent<IKControl>();

        _KeyUI = GameObject.FindGameObjectWithTag("KeyUI");
        if (_KeyUI != null)
        {
            _KeyText = _KeyUI.GetComponentInChildren<TextMeshProUGUI>();
        }

        _KeyUI.SetActive(false);
    }

    /*    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Dragable"))
            {
                _currentObject = other.gameObject;
                if (_isHandEmpty)
                {
                    _grabUI.SetActive(true);
                    _putUI.SetActive(false);
                }
                else if (!_isHandEmpty)
                {
                    _grabUI.SetActive(false);
                    _putUI.SetActive(true);
                }
            }
        }*/
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable"))
        {
            _currentObject = other.gameObject;

            if (_isHandEmpty)
            {
                _KeyUI.SetActive(true);
                _KeyText.text = "Pick Up";
            }
            else if (!_isHandEmpty)
            {
                _KeyUI.SetActive(true);
                _KeyText.text = "Drop";
            }
        }
    }

    public void GrabOrPut(InputAction.CallbackContext context)
    {
        if (!isLocalPlayer) return;

        if (context.started)
        {
            if (_isHandEmpty && _currentObject != null)
            {
                CmdGrabObject(_currentObject);
            }
            else if (!_isHandEmpty && _currentObject)
            {
                CmdReleaseObject();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable") && _isHandEmpty)
        {
            _currentObject = null;
            _KeyUI.SetActive(false);
        }
    }

    private void SetIKControl(bool switcher)
    {
        if(switcher)
        {
            _currentObjLeft = _currentObject.GetComponent<Dragable>().PointLeft;
            _currentObjRight = _currentObject.GetComponent<Dragable>().PointRight;
        }
        else
        {
            _currentObjLeft = null;
            _currentObjRight = null;
        }
        _ikcontrol.SetIKActive(switcher);
        gameObject.GetComponent<IKControl>().HandObjLeft = _currentObjLeft;
        gameObject.GetComponent<IKControl>().HandObjRight = _currentObjRight;
    }

    //Mirror Scripts
    [Command]
    private void CmdGrabObject(GameObject objectToGrab)
    {
        RpcGrabObject(objectToGrab);
    }

    [Command]
    private void CmdReleaseObject()
    {
        RpcReleaseObject();
    }

    [ClientRpc]
    private void RpcGrabObject(GameObject objectToGrab)
    {
        if (_isHandEmpty)
        {
            Debug.Log("Grab | Player Net ID: " + netId);
            _isHandEmpty = false;
            SetIKControl(true);

            _currentObject = objectToGrab;

            _currentObject.transform.parent = _jointHolder.transform;
            _currentObject.transform.position = _jointHolder.transform.position;
            _currentObject.transform.rotation = _jointHolder.transform.rotation;

            _rb = _currentObject.GetComponent<Rigidbody>();
            if (_rb != null)
            {
                _rb.isKinematic = true;
            }

            //_currentObject.GetComponent<Dragable>().required = _isHandEmpty;
        }
    }

    [ClientRpc]
    private void RpcReleaseObject()
    {
        if (!_isHandEmpty)
        {
            Debug.Log("Put | Player Net ID: " + netId);
            _isHandEmpty = true;
            SetIKControl(false);

            _currentObject.transform.parent = null;

            if (_rb != null)
            {
                _rb.isKinematic = false;
                _rb = null; // Reset Rigidbody reference
            }

            _currentObject = null;
        }
    }
}
