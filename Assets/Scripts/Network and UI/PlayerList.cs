using Mirror;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    [SerializeField] private GameObject[] _players;
    [SerializeField] private NetworkRoomManager _roomManager;
    private void Awake()
    {
        _roomManager = FindAnyObjectByType<NetworkRoomManager>();

        for (int i = 0; i < _roomManager.roomSlots.Count; i++)
        {
            _players[i].SetActive(true);
            //_players[i].GetComponentInChildren<TextMeshProUGUI>().color = _roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().color;
            _players[i].GetComponentInChildren<TextMeshProUGUI>().text = _roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().playerName;
        }
    }
}
