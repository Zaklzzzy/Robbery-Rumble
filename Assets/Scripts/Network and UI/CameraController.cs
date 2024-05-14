using UnityEngine;
using Mirror;
using Cinemachine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private GameObject[] _players;
    [SerializeField] private CinemachineVirtualCamera[] _cameras;

    private void Update()
    {
        //Set Material To Players
        _players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < _players.Length; i++)
        {
            _cameras[i].m_Follow = _players[i].transform;
        }
    }
}
