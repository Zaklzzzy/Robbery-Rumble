using Mirror;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VanGarbage : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private int _maxCount;
    private int _count;

    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();

    private void Start()
    {
        _maxCount = FindObjectsByType<Dragable>(FindObjectsSortMode.None).Where(obj => obj.required == true).ToArray().Length;
        ChangeText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dragable") && other.GetComponent<Dragable>().required)
        {
            if (objectsInTrigger.Add(other.gameObject) && isServer)
            {
                _count++;
                ChangeText();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dragable") && other.GetComponent<Dragable>().required)
        {
            if (objectsInTrigger.Remove(other.gameObject) && isServer)
            {
                _count--;
                ChangeText();
            }
        }
    }

    private void ChangeText()
    {
        _countText.text = "Objects: " + _count + "/" + _maxCount;
    }
}