using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public int winScores;
    public HitEvents hitEvents;
    public ScoreEvents scoreEvents;
    public GameEvents gameEvents;
    
    private readonly Dictionary<NetworkIdentity, int> _playerScores = new ();
    
    [Server]
    private void Start()
    {
        hitEvents.HitEvent += AddScore;
        scoreEvents.RegisterPlayerEvent += OnRegisterPlayerEvent;
        scoreEvents.UnregisterPlayerEvent += OnUnregisterPlayerEvent;
        gameEvents.ResetScoresEvent += ResetScores;
    }
    
    [Server]
    private void OnRegisterPlayerEvent(NetworkIdentity playerId)
    {
        _playerScores.Add(playerId, 0);
    }
    
    [Server]
    private void OnUnregisterPlayerEvent(NetworkIdentity playerId)
    {
        _playerScores.Remove(playerId);
    }
    
    [Server]
    private void AddScore(NetworkIdentity playerId)
    {
        if (!_playerScores.TryGetValue(playerId, out var score)) return;
        
        var newScore = score + 1;
        _playerScores[playerId] = newScore;
        scoreEvents.UpdateScore(playerId, newScore);
        
        if (newScore >= winScores) gameEvents.PlayerWon(playerId);
    }

    [Server]
    private void ResetScores()
    {
        foreach (var playerId in _playerScores.Keys.ToList())
        {
            _playerScores[playerId] = 0;
            scoreEvents.UpdateScore(playerId, 0);
        }
    }
}
