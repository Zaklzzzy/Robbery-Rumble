using Mirror;
using System.Collections;
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

    public NetworkRoomManager _roomManager;
    private bool sceneChanging = false;

    private void Start()
    {
        SetRandomRequiredObjects();
        _maxCount = FindObjectsByType<Dragable>(FindObjectsSortMode.None).Where(obj => obj.required == true).ToArray().Length;
        ChangeText();

        _roomManager = FindAnyObjectByType<NetworkRoomManager>();
    }

    private void SetRandomRequiredObjects()
    {
        // Find all Dragable objects
        Dragable[] allDragableObjects = FindObjectsByType<Dragable>(FindObjectsSortMode.None);

        // Get a random number between 3 and 5
        int requiredCount = Random.Range(3, 6);

        allDragableObjects = allDragableObjects.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < requiredCount; i++)
        {
            allDragableObjects[i].required = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dragable") && other.GetComponent<Dragable>().required)
        {
            if (objectsInTrigger.Add(other.gameObject) && isServer)
            {
                _count++;
                ChangeText();

                if (_count == _maxCount && !sceneChanging)
                {
                    sceneChanging = true;
                    StartCoroutine(ChangeSceneAfterDelay());
                }
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

    [Server]
    private IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            if (!conn.isReady)
            {
                conn.Send(new NetworkPingMessage());
                yield return new WaitForSeconds(0.5f);
            }
        }

        if (sceneChanging)
        {
            _roomManager.ServerChangeScene("StartMenu");
        }
    }
}