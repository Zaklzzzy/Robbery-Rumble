using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPick : MonoBehaviour
{
    private Animator _animator;
    private Transform currentInteractionObjLeft;
    private Transform currentInteractionObjRight;

    [SerializeField] private IKControl _ikcontrol;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dragable"))
        {
            _ikcontrol.SetIKActive(true);

            currentInteractionObjLeft = other.gameObject.GetComponent<Dragable>().PointLeft;
            currentInteractionObjRight = other.gameObject.GetComponent<Dragable>().PointRight;

            gameObject.GetComponent<IKControl>().HandObjLeft = currentInteractionObjLeft;
            gameObject.GetComponent<IKControl>().HandObjRight = currentInteractionObjRight;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("Dragable") && other.gameObject.GetComponent<Dragable>().interactionPointLeft == currentInteractionObj)
        if (other.gameObject.CompareTag("Dragable"))
        {
            currentInteractionObjLeft = null;
            currentInteractionObjRight = null;

            _ikcontrol.SetIKActive(false);

            gameObject.GetComponent<IKControl>().HandObjLeft = currentInteractionObjLeft;
            gameObject.GetComponent<IKControl>().HandObjRight = currentInteractionObjRight;
        }
    }
}
