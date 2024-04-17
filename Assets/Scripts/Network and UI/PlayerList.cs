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
            //_players[i].GetComponentInChildren<TextMeshProUGUI>().color = ParseColor(_roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().color);
            _players[i].GetComponentInChildren<TextMeshProUGUI>().text = _roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().playerName;
        }
    }

    private Color ParseColor(string color)
    {
        if(ColorUtility.TryParseHtmlString(color, out Color outputColor)) return outputColor;
        Debug.Log("Error to Parse Color");
        return Color.white;
    }
}
