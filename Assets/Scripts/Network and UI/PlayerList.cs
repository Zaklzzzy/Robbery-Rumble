using Mirror;
using TMPro;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
    [SerializeField] private GameObject[] _playersUI;
    [SerializeField] private GameObject[] _players;
    [SerializeField] private Material[] _materials;
    [SerializeField] private NetworkRoomManager _roomManager;
    private void Start()
    {
        //Find Room Manager
        _roomManager = FindAnyObjectByType<NetworkRoomManager>();

        //Set UI List
        for (int i = 0; i < _roomManager.roomSlots.Count; i++)
        {
            _playersUI[i].SetActive(true);
            //_players[i].GetComponentInChildren<TextMeshProUGUI>().color = ParseColor(_roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().color);
            _playersUI[i].GetComponentInChildren<TextMeshProUGUI>().text = _roomManager.roomSlots[i].GetComponent<RoomPlayerUI>().playerName;
        }
    }

    //Change to NonUpdateFunction
    private void Update()
    {
        //Set Material To Players
        _players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].GetComponentInChildren<SkinnedMeshRenderer>().material = _materials[i];
        }
    }

    private Color ParseColor(string color)
    {
        if(ColorUtility.TryParseHtmlString(color, out Color outputColor)) return outputColor;
        Debug.Log("Error to Parse Color");
        return Color.white;
    }
}
