using System.Collections.Generic;
using System.Linq;
using Events;
using Mirror;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public int winScores;
    public PlayerHitEvents playerHitEvents;
    public ScoreEvents scoreEvents;
    public GameEvents gameEvents;
    
    private readonly Dictionary<NetworkIdentity, int> _playerScores = new ();
    
    [Server]
    private void Start()
    {
        if (!playerHitEvents) Debug.LogError("playerHitEvents is not set");
        if (!scoreEvents) Debug.LogError("scoreEvents is not set");
        if (!gameEvents) Debug.LogError("gameEvents is not set");
        
        playerHitEvents.HitEvent += AddScore;
        scoreEvents.RegisterPlayerEvent += OnRegisterPlayerEvent;
        scoreEvents.UnregisterPlayerEvent += OnUnregisterPlayerEvent;
        gameEvents.ResetScoresEvent += ResetScores;
    }
    
    private void OnRegisterPlayerEvent(NetworkIdentity playerId)
    {
        _playerScores.Add(playerId, 0);
    }
    
    private void OnUnregisterPlayerEvent(NetworkIdentity playerId)
    {
        _playerScores.Remove(playerId);
    }
    
    private void AddScore(NetworkIdentity playerId)
    {
        if (!_playerScores.TryGetValue(playerId, out var score)) return;
        
        var newScore = score + 1;
        _playerScores[playerId] = newScore;
        scoreEvents.UpdateScore(playerId, newScore);
        
        if (newScore >= winScores) gameEvents.PlayerWon(playerId);
    }
    
    private void ResetScores()
    {
        foreach (var playerId in _playerScores.Keys.ToList())
        {
            _playerScores[playerId] = 0;
            scoreEvents.UpdateScore(playerId, 0);
        }
    }
    
    private void OnDestroy()
    {
        playerHitEvents.HitEvent -= AddScore;
        scoreEvents.RegisterPlayerEvent -= OnRegisterPlayerEvent;
        scoreEvents.UnregisterPlayerEvent -= OnUnregisterPlayerEvent;
        gameEvents.ResetScoresEvent -= ResetScores;
    }
}
