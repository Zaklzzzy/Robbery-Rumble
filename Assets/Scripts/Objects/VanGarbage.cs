using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class VanGarbage : NetworkBehaviour
{
    [Header("Objects Count")]
    [SerializeField] private TextMeshProUGUI _countTextNow;
    [SerializeField] private TextMeshProUGUI _countTextMax;
    [SerializeField] private int _maxCount;
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float _gameDuration = 120f; // Game Time
    [Header("Audio")]
    [SerializeField] private AudioSourceCreator _audioSourceHandler;
    [SerializeField] private AudioResource _winMusic;

    private int _count;
    private float _timeRemaining;
    private bool _isGameOver = false;

    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();

    [Header("Network")]
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

    private void Update()
    {
        _maxCount = FindObjectsByType<Dragable>(FindObjectsSortMode.None).Where(obj => obj.required == true).ToArray().Length;

        EnableOutlineForRequiredObjects();
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
                    _audioSourceHandler.CreateAudioSource(_winMusic);
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
        _countTextNow.text = _count.ToString();
        _countTextMax.text =  _maxCount.ToString();
    }

    [Server]
    private IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        foreach (var conn in NetworkServer.connections.Values)
        {
            if (!conn.isReady)
            {
                conn.Send(new NetworkPingMessage());
                yield return new WaitForSeconds(5f);
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
    private void EnableOutlineForRequiredObjects()
    {
        var requiredObjects = FindObjectsByType<Dragable>(FindObjectsSortMode.None).Where(obj => obj.required == true);
        foreach (var obj in requiredObjects)
        {
            EnableOutline(obj.gameObject);
        }
    }
}
