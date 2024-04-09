using Mirror;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _players;
    [SerializeField] private NetworkRoomManager _roomManager;

    private void Awake()
    {
        _roomManager = FindAnyObjectByType<NetworkRoomManager>();
    }

    private void Update()
    {
        for (int i = 0; i < _roomManager.roomSlots.Count; i++)
        {
            _players[i].SetActive(true);
        }
    }
}
