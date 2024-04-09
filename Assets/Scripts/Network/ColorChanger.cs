using Mirror;
using UnityEngine;

public class ColorChanger : NetworkBehaviour
{
    [SerializeField] private Material _material;

    [SyncVar(hook = nameof(OnColorChange))]
    public Color _color;

    public override void OnStartServer()
    {
        _color = Color.white;
    }

    //Update Color On Clients
    private void OnColorChange(Color oldColor, Color newColor)
    {
        _material.color = newColor;
    }

    //Update Color On Server
    [Command]
    public void CmdChangeColor(Color newColor)
    {
        _color = newColor;
    }

    //Color Functions
    public void ChangeColorRed()
    {
        if (isLocalPlayer)
        {
            CmdChangeColor(Color.red);
        }
    }
    public void ChangeColorGreen()
    {
        if (isLocalPlayer)
        {
            CmdChangeColor(Color.green);
        }
    }
    public void ChangeColorBlue()
    {
        if (isLocalPlayer)
        {
            CmdChangeColor(Color.blue);
        }
    }
}
    