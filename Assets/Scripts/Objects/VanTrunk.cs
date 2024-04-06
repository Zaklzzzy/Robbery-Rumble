using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VanGarbage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private int _maxCount;
    private int _count;

    private void Start()
    {
        _countText.text = "Objects: 0/" + _maxCount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Dragable") && other.GetComponent<Dragable>().required)
        {
            _count++;
            _countText.text = "Objects: " + _count + "/" + _maxCount;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dragable") && other.GetComponent<Dragable>().required)
        {
            _count--;
            _countText.text = "Objects: " + _count + "/" + _maxCount;
        }
    }
}
