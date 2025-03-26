using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab; // Assign in Inspector
    [SerializeField] private Transform[] spawnPoints; // Multiple spawn locations

    private Dictionary<ulong, GameObject> spawnedPlayers = new Dictionary<ulong, GameObject>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        SpawnPlayerServerRpc(clientId);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (spawnedPlayers.TryGetValue(clientId, out GameObject player))
        {
            Destroy(player);
            spawnedPlayers.Remove(clientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong clientId)
    {
        if (spawnedPlayers.ContainsKey(clientId)) return; // Prevent multiple spawns

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; // Random spawn

        GameObject playerInstance = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // Assign ownership

        spawnedPlayers[clientId] = playerInstance;
    }
}
