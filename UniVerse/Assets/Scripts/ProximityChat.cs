using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class ProximityChat : NetworkBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private float chatRadius = 5f;

    private static List<ProximityChat> allPlayers = new List<ProximityChat>();

    private void OnEnable()
    {
        if (isServer)
            allPlayers.Add(this);
    }

    private void OnDisable()
    {
        if (isServer)
            allPlayers.Remove(this);
    }

    [Server]
    private void FixedUpdate()
    {
        CheckProximity();
    }

    [Server]
    private void CheckProximity()
    {
        foreach (var player in allPlayers)
        {
            foreach (var other in allPlayers)
            {
                if (player == other) continue;

                bool isVisible = Vector3.Distance(player.transform.position, other.transform.position) < chatRadius;
                player.TargetSetBubbleVisibility(other.connectionToClient, isVisible);
            }
        }
    }

    [TargetRpc]
    private void TargetSetBubbleVisibility(NetworkConnection target, bool isVisible)
    {
        if (speechBubble != null)
            speechBubble.SetActive(isVisible);
    }
}

