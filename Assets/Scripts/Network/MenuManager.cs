using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkManager))]

public class MenuManager : MonoBehaviour
{
    NetworkManager manager;

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    // �������� ��� ������ Host (Server + Client)
    public void StartHost()
    {
        if (!NetworkClient.active)
        {
            manager.StartHost();
        }
    }

    // �������� ��� ������ Client
    public void StartClient()
    {
        if (!NetworkClient.active)
        {
            manager.StartClient();
        }
    }

    // �������� ��� ������ Server
    public void StartServer()
    {
        if (!NetworkClient.active)
        {
            manager.StartServer();
        }
    }

    // �������� ��� ��������� Host
    public void StopHost()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
        }
    }

    // �������� ��� ��������� Client
    public void StopClient()
    {
        if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
    }

    // �������� ��� ��������� Server
    public void StopServer()
    {
        if (NetworkServer.active)
        {
            manager.StopServer();
        }
    }

    // �������� ���� �����, ����� �������� ����� �������
    public void SetServerAddress(string address)
    {
        manager.networkAddress = address;
    }

    // �����������: �������� ���� ����� ��� ��������� �����, ���� ����������� PortTransport
    public void SetServerPort(string portString)
    {
        if (ushort.TryParse(portString, out ushort port))
        {
            if (Transport.active is PortTransport portTransport)
            {
                portTransport.Port = port;
            }
        }
    }

    private void OnDestroy()
    {
        if (NetworkServer.active)
        {
            manager.StopServer();
        }

        if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
    }
}