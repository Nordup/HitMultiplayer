using System.Linq;
using HitScriptableObjects;
using Mirror;
using UnityEngine;

public class RespawnPlayers : NetworkBehaviour
{
    public GameEvents gameEvents;
    
    private void Start()
    {
        if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
        
        gameEvents.PlayerWonEvent += DestroyPlayerObjects;
        gameEvents.RestartMatchEvent += Respawn;
    }
    
    private void DestroyPlayerObjects(NetworkIdentity obj)
    {
        foreach (var conn in NetworkServer.connections.Values.ToArray())
        {
            Destroy(conn.identity.gameObject);
        }
    }
    
    private void Respawn()
    {
        foreach (var conn in NetworkServer.connections.Values.ToArray())
        {
            var newTransform = NetworkManager.singleton.GetStartPosition();
            NetworkServer.ReplacePlayerForConnection(conn,
                Instantiate(NetworkManager.singleton.playerPrefab, newTransform.position, newTransform.rotation), true);
        }
    }
    
    private void OnDestroy()
    {
        gameEvents.PlayerWonEvent -= DestroyPlayerObjects;
        gameEvents.RestartMatchEvent -= Respawn;
    }
}