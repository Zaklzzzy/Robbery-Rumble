using Mirror;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(NetworkManager))]

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject settingUI;

    NetworkManager manager;

    private void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    // Действие для старта Host (Server + Client)
    public void StartHost()
    {
        if (!NetworkClient.active)
        {
            manager.StartHost();
        }
    }

    // Действие для старта Client
    public void StartClient()
    {
        if (!NetworkClient.active)
        {
            manager.StartClient();
        }
    }

    // Действие для старта Server
    public void StartServer()
    {
        if (!NetworkClient.active)
        {
            manager.StartServer();
        }
    }

    // Действие для остановки Host
    public void StopHost()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
        }
    }

    // Действие для остановки Client
    public void StopClient()
    {
        if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
    }

    // Действие для остановки Server
    public void StopServer()
    {
        if (NetworkServer.active)
        {
            manager.StopServer();
        }
    }

    // Изменение адреса сервера
    public void SetServerAddress(string address)
    {
        manager.networkAddress = address;
    }

    // Опционально: метод для изменения порта, если используется PortTransport
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
    
    // Открытие настроек
    public void SettingUI(bool switcher)
    {
        settingUI.SetActive(switcher);
        menuUI.SetActive(!switcher);
    }

    // Выход из игры
    public void QuitGame()
    {
        Application.Quit();
    }

    // Остановка сервера при выходе
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