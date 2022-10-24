using System.Collections.Generic;
using Events;
using Mirror;
using UnityEngine;

public class PlayersManager : NetworkBehaviour
{
    public GameEvents gameEvents;
    
    private readonly Dictionary<NetworkIdentity, string> _players = new();
    
    private void Start()
    {
        if (!gameEvents) Debug.LogError("gameEvents is not set");
        
        gameEvents.PlayerJoinedEvent += OnPlayerJoined;
        gameEvents.PlayerLeftEvent += OnPlayerLeft;
    }
    
    public string GetPlayerName(NetworkIdentity playerId)
    {
        return _players.TryGetValue(playerId, out var playerName) ? playerName : "";
    }
    
    private void OnPlayerJoined(NetworkIdentity playerId, string playerName)
    {
        _players.Add(playerId, playerName);
    }
    
    private void OnPlayerLeft(NetworkIdentity playerId)
    {
        if (!_players.TryGetValue(playerId, out var playerName))
        {
            Debug.LogError($"Not found client {playerId}");
            return;
        }
        
        _players.Remove(playerId);
    }
    
    private void OnDestroy()
    {
        gameEvents.PlayerJoinedEvent -= OnPlayerJoined;
        gameEvents.PlayerLeftEvent -= OnPlayerLeft;
    }
}