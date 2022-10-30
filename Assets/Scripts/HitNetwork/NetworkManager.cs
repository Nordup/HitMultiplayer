using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace HitNetwork
{
    [RequireComponent(typeof(NetworkDiscovery))]
    public class NetworkManager : Mirror.NetworkManager
    {
        public GameEvents gameEvents;
        public NetworkEvents networkEvents;
        
        private NetworkDiscovery _networkDiscovery;
        
        private bool _playerSpawningAllowed;
        
        public override void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            
            _networkDiscovery = GetComponent<NetworkDiscovery>();
            _playerSpawningAllowed = true;
            gameEvents.PlayerWonEvent += DisableSpawning;
            gameEvents.RestartMatchEvent += EnableSpawning;
            
            base.Start();
        }
        
        private void DisableSpawning(NetworkIdentity playerId) =>_playerSpawningAllowed = false;
        private void EnableSpawning() => _playerSpawningAllowed = true;
        
        // Server
        
        public override void OnStartServer()
        {
            _networkDiscovery.AdvertiseServer();
        }
        
        public override void OnStopServer()
        {
            _networkDiscovery.StopDiscovery();
        }
        
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            if (!_playerSpawningAllowed) return;
            
            var startPos = GetStartPosition();
            var player = startPos != null
                ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
                : Instantiate(playerPrefab);
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            NetworkServer.AddPlayerForConnection(conn, player);
        }
        
        // Client
        
        public override void OnClientConnect()
        {
            networkEvents.ClientConnect();
            
            NetworkClient.Ready();
            if (NetworkClient.localPlayer == null)
            {
                NetworkClient.AddPlayer();
            }
        }
        
        public override void OnClientDisconnect()
        {
            NetworkClient.Shutdown();
            networkEvents.ClientDisconnect();
        }
        
        public override void OnDestroy()
        {
            gameEvents.PlayerWonEvent -= DisableSpawning;
            gameEvents.RestartMatchEvent -= EnableSpawning;
            base.OnDestroy();
        }
    }
}