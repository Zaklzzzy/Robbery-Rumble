using Mirror;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    //[SerializeField] private Camera playerCamera;
    [SerializeField] private Canvas playerCanvas;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        //playerCamera.enabled = true;
        playerCanvas.enabled = true;
    }
}
