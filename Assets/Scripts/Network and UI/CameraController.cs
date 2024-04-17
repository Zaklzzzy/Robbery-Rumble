using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    private void Awake()
    {
        if (!isLocalPlayer)
        {
            gameObject.SetActive(false);

        }
        else
        {
            gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
