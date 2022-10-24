using System.Linq;
using Events;
using Mirror;
using UnityEngine;

public class RespawnPlayers : NetworkBehaviour
{
    public GameEvents gameEvents;
    public NetworkManager manager;
    
    private void Start()
    {
        if (!gameEvents) Debug.LogError("gameEvents is not set");
        gameEvents.RestartMatchEvent += Respawn;
    }
    
    [Server]
    private void Respawn()
    {
        foreach (var conn in NetworkServer.connections.Values.ToArray())
        {
            var oldPrefab = conn.identity.gameObject;
            var newTransform = manager.GetStartPosition();
            NetworkServer.ReplacePlayerForConnection(conn,
                Instantiate(manager.playerPrefab, newTransform.position, newTransform.rotation), true);
            Destroy(oldPrefab);
        }
    }
    
    private void OnDestroy()
    {
        gameEvents.RestartMatchEvent -= Respawn;
    }
}