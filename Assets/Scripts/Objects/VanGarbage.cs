using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VanGarbage : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private int _maxCount;
    [SerializeField] private float _gameDuration = 300f; // Game Time
    private int _count;
    private float _timeRemaining;
    private bool _isGameOver = false;

    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();

    public NetworkRoomManager _roomManager;
    private bool sceneChanging = false;

    private void Start()
    {
        if (isServer)
        {
            SetRandomRequiredObjects();
        }

        _maxCount = FindObjectsByType<Dragable>(FindObjectsSortMode.None).Where(obj => obj.required == true).ToArray().Length;
        ChangeText();
        _roomManager = FindAnyObjectByType<NetworkRoomManager>();

        // Init timer
        _timeRemaining = _gameDuration;
        StartCoroutine(GameTimer());
    }

    [Server]
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
            EnableOutline(allDragableObjects[i].gameObject);
            RpcSetRequired(allDragableObjects[i].gameObject, true);
        }
    }

    [ClientRpc]
    private void RpcSetRequired(GameObject obj, bool isRequired)
    {
        if (obj.TryGetComponent(out Dragable dragable))
        {
            dragable.required = isRequired;
            if (isRequired)
            {
                EnableOutline(obj);
            }
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
                RpcChangeText(_count);

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
                RpcChangeText(_count);
            }
        }
    }

    private void ChangeText()
    {
        _countText.text = "Objects: " + _count + "/" + _maxCount;
    }

    [ClientRpc]
    private void RpcChangeText(int count)
    {
        _count = count;
        ChangeText();
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

    [Server]
    private IEnumerator GameTimer()
    {
        while (_timeRemaining > 0 && !_isGameOver)
        {
            yield return new WaitForSeconds(1f);
            _timeRemaining--;

            // Update Timer UI
            RpcUpdateTimerText(_timeRemaining);

            if (_timeRemaining <= 0)
            {
                _isGameOver = true;
                _roomManager.ServerChangeScene("StartMenu");
            }
        }
    }

    [ClientRpc]
    private void RpcUpdateTimerText(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EnableOutline(GameObject obj)
    {
        var outline = obj.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }
}
