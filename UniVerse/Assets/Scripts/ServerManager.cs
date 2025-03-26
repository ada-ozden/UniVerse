using Unity.Netcode;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private void Start()
    {
        if (Application.isBatchMode) // Server mode
        {
            NetworkManager.Singleton.StartServer();
        }
        else
        {
            Debug.LogError("ServerManager script should only be used in headless server builds.");
        }
    }
}
