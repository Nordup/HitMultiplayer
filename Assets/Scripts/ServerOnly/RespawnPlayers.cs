using System.Linq;
using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace ServerOnly
{
    public class RespawnPlayers : NetworkBehaviour
    {
        public GameEvents gameEvents;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            
            gameEvents.PlayerWonEvent += DestroyPlayerObjects;
            gameEvents.RestartMatchEvent += Respawn;
        }
        
        private void DestroyPlayerObjects(NetworkIdentity playerId)
        {
            NetworkManager.singleton.autoCreatePlayer = false;
            foreach (var conn in NetworkServer.connections.Values.ToArray())
            {
                Destroy(conn.identity.gameObject);
            }
        }
        
        private void Respawn()
        {
            NetworkManager.singleton.autoCreatePlayer = true;
            foreach (var conn in NetworkServer.connections.Values.ToArray())
            {
                var newTransform = NetworkManager.singleton.GetStartPosition();
                var playerPrefab = NetworkManager.singleton.playerPrefab;
                var player = Instantiate(playerPrefab, newTransform.position, newTransform.rotation);
                player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
                
                if (conn.identity == null)
                    NetworkServer.AddPlayerForConnection(conn, player);
                else
                    NetworkServer.ReplacePlayerForConnection(conn, player, true);
            }
        }
        
        private void OnDestroy()
        {
            gameEvents.PlayerWonEvent -= DestroyPlayerObjects;
            gameEvents.RestartMatchEvent -= Respawn;
        }
    }
}