using Mirror;
using TMPro;
using UnityEngine;

public class RoomPlayerUI : NetworkBehaviour
{
    //public TMP_Text playerNameText;
    //public TMP_Text playerReadyText;  

    [SyncVar(hook = nameof(OnNameChanged))]
    public string playerName;

    [SyncVar(hook = nameof(OnReadyStatusChanged))]
    public bool isReady;

/*    [SyncVar(hook = nameof(OnReadyStatusChanged))]
    public byte color;*/

    private void OnNameChanged(string _Old, string _New)
    {
        //playerNameText.text = playerName;
    }

    private void OnReadyStatusChanged(bool _Old, bool _New)
    {
        //playerReadyText.text = isReady ? "Ready" : "Not Ready";
    }

/*    private void OnColor(byte _Old, byte _New)
    {
        //playerReadyText.text = isReady ? "Ready" : "Not Ready";
    }*/

    [Command]
    public void CmdSetReady()
    {
        isReady = !isReady;
    }
}
