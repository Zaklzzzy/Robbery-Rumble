using UnityEngine;
using Mirror;
using Cinemachine;
using System.Collections.Generic;
    
public class CameraController : NetworkBehaviour
{
    private static List<CinemachineVirtualCamera> availableCameras = new List<CinemachineVirtualCamera>();
    private CinemachineVirtualCamera playerCamera;

    private void Start()
    {
        if (!isLocalPlayer) return;

        // Find All Virtual Cameras
        if (availableCameras.Count == 0)
        {
            availableCameras.AddRange(FindObjectsByType<CinemachineVirtualCamera>(FindObjectsSortMode.None));
            foreach (var vcam in availableCameras)
            {
                vcam.gameObject.SetActive(false);
            }
        }

        // Avaiblable one camera for player
        if (availableCameras.Count > 0)
        {
            playerCamera = availableCameras[0];
            availableCameras.RemoveAt(0);

            playerCamera.Follow = transform;
            playerCamera.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        // Make camera avaiblable on destroy player
        if (playerCamera != null)
        {
            availableCameras.Add(playerCamera);
            playerCamera.gameObject.SetActive(false);
        }
    }
}
