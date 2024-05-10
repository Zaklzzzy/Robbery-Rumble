using Mirror;
using UnityEngine;

public class GeneralUIEnable : NetworkBehaviour
{
    private void Start()
    {
        if (isServer)
        {
            gameObject.SetActive(true);
        }
    }

}
