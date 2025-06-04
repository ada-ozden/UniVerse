namespace PlayFab.Networking
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Mirror;
    using UnityEngine.Events;

    public class UnityNetworkServer : NetworkManager
    {
        public static UnityNetworkServer Instance { get; private set; }

        [Header("Custom Settings")]
        public Transform customSpawnPoint; // ✅ assignable in inspector
        public int Port = 7777; // overwritten by AgentListener.cs

        public PlayerEvent OnPlayerAdded = new PlayerEvent();
        public PlayerEvent OnPlayerRemoved = new PlayerEvent();

        public List<UnityNetworkConnection> Connections
        {
            get { return _connections; }
            private set { _connections = value; }
        }
        private List<UnityNetworkConnection> _connections = new List<UnityNetworkConnection>();

        public class PlayerEvent : UnityEvent<string> { }

        public override void Awake()
        {
            base.Awake();
            Instance = this;
            NetworkServer.RegisterHandler<ReceiveAuthenticateMessage>(OnReceiveAuthenticate);
        }

        public void StartListen()
        {
            this.GetComponent<kcp2k.KcpTransport>().port = (ushort)Port;
            NetworkServer.Listen(maxConnections);
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            NetworkServer.Shutdown();
        }

        private void OnReceiveAuthenticate(NetworkConnectionToClient conn, ReceiveAuthenticateMessage message)
        {
            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn != null)
            {
                uconn.PlayFabId = message.PlayFabId;
                uconn.IsAuthenticated = true;
                OnPlayerAdded.Invoke(message.PlayFabId);
            }
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);

            Debug.LogWarning("Client Connected");
            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn == null)
            {
                _connections.Add(new UnityNetworkConnection()
                {
                    Connection = conn,
                    ConnectionId = conn.connectionId,
                    LobbyId = PlayFabMultiplayerAgentAPI.SessionConfig.SessionId
                });
            }
        }

        public override void OnServerError(NetworkConnectionToClient conn, TransportError error, string reason)
        {
            base.OnServerError(conn, error, reason);
            Debug.Log($"Unity Network Connection Status: exception - {reason}");
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);

            var uconn = _connections.Find(c => c.ConnectionId == conn.connectionId);
            if (uconn != null)
            {
                if (!string.IsNullOrEmpty(uconn.PlayFabId))
                {
                    OnPlayerRemoved.Invoke(uconn.PlayFabId);
                }
                _connections.Remove(uconn);
            }
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Transform startPos = customSpawnPoint != null ? customSpawnPoint : GetStartPosition();
            GameObject player = Instantiate(playerPrefab, startPos.position, startPos.rotation);
            NetworkServer.AddPlayerForConnection(conn, player);
        }
    }

    [Serializable]
    public class UnityNetworkConnection
    {
        public bool IsAuthenticated;
        public string PlayFabId;
        public string LobbyId;
        public int ConnectionId;
        public NetworkConnectionToClient Connection;
    }

    public class CustomGameServerMessageTypes
    {
        public const short ReceiveAuthenticate = 900;
        public const short ShutdownMessage = 901;
        public const short MaintenanceMessage = 902;
    }

    public struct ReceiveAuthenticateMessage : NetworkMessage
    {
        public string PlayFabId;
    }

    public struct ShutdownMessage : NetworkMessage { }

    [Serializable]
    public struct MaintenanceMessage : NetworkMessage
    {
        public DateTime ScheduledMaintenanceUTC;
    }
}