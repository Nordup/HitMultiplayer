using System.Linq;
using HitScriptableObjects;
using Mirror;
using UnityEngine;

public class RespawnPlayers : NetworkBehaviour
{
    public GameEvents gameEvents;
    public NetworkManager manager;
    
    private void Start()
    {
        if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
        gameEvents.PlayerWonEvent += UnSpawn;
        gameEvents.RestartMatchEvent += Respawn;
    }
    
    private void UnSpawn(NetworkIdentity obj)
    {
        foreach (var conn in NetworkServer.connections.Values.ToArray())
        {
            var oldPrefab = conn.identity.gameObject;
            Destroy(oldPrefab);
        }
    }
    
    private void Respawn()
    {
        foreach (var conn in NetworkServer.connections.Values.ToArray())
        {
            var newTransform = manager.GetStartPosition();
            NetworkServer.ReplacePlayerForConnection(conn,
                Instantiate(manager.playerPrefab, newTransform.position, newTransform.rotation), true);
        }
    }
    
    private void OnDestroy()
    {
        gameEvents.PlayerWonEvent -= UnSpawn;
        gameEvents.RestartMatchEvent -= Respawn;
    }
}