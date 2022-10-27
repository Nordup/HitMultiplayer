using System.Collections.Generic;
using System.Linq;
using HitScriptableObjects;
using Mirror;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public int winScores;
    public GameEvents gameEvents;
    
    private readonly Dictionary<NetworkIdentity, int> _playerScores = new ();
    
    [Server]
    private void Start()
    {
        if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
        
        gameEvents.PlayerJoinedEvent += RegisterPlayer;
        gameEvents.PlayerLeftEvent += UnregisterPlayer;
        gameEvents.PlayerHitEvent += AddScore;
        gameEvents.RestartMatchEvent += ClearScores;
    }

    public int GetPlayerScore(NetworkIdentity playerId)
    {
        return _playerScores.TryGetValue(playerId, out var score) ? score : -1;
    }
    
    private void RegisterPlayer(NetworkIdentity playerId, string playerName)
    {
        _playerScores.Add(playerId, 0);
    }
    
    private void UnregisterPlayer(NetworkIdentity playerId)
    {
        _playerScores.Remove(playerId);
    }
    
    private void AddScore(NetworkIdentity playerId)
    {
        if (!_playerScores.TryGetValue(playerId, out var score)) return;
        
        var newScore = score + 1;
        _playerScores[playerId] = newScore;
        gameEvents.UpdatePlayerScore(playerId, newScore);
        
        if (newScore >= winScores) gameEvents.PlayerWon(playerId);
    }
    
    private void ClearScores()
    {
        foreach (var playerId in _playerScores.Keys.ToList())
        {
            _playerScores[playerId] = 0;
            gameEvents.UpdatePlayerScore(playerId, 0);
        }
    }
    
    private void OnDestroy()
    {
        gameEvents.PlayerJoinedEvent -= RegisterPlayer;
        gameEvents.PlayerLeftEvent -= UnregisterPlayer;
        gameEvents.PlayerHitEvent -= AddScore;
        gameEvents.RestartMatchEvent -= ClearScores;
    }
}
