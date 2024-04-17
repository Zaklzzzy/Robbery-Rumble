using Mirror;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _players;
    [SerializeField] private GameObject[] _UI;
    public NetworkRoomManager _roomManager;

    private void Awake()
    {
        _roomManager = FindAnyObjectByType<NetworkRoomManager>();
    }

    private void Update()
    {
        for (int i = 0; i < _roomManager.roomSlots.Count; i++)
        {
            _players[i].SetActive(true);
            _UI[i].SetActive(true);
            _UI[i].GetComponentInChildren<TextMeshProUGUI>().text = _roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().playerName;
            if(_roomManager.roomSlots[i].GetComponent<NetworkRoomPlayer>().readyToBegin)
            {
                _UI[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
            }
            else
            {
                _UI[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
        }
    }

    public void ReadySwitch(int index)
    {
        if (_roomManager.roomSlots[index - 1].GetComponent<NetworkRoomPlayer>().readyToBegin)
        {
            _roomManager.roomSlots[index - 1].GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(false);
            _UI[index - 1].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            _roomManager.roomSlots[index - 1].GetComponent<NetworkRoomPlayer>().CmdChangeReadyState(true);
            _UI[index - 1].GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        }
    }
}