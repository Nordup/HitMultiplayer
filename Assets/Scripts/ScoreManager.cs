using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public int winScores;
    public HitEvents hitEvents;
    public ScoreEvents scoreEvents;
    
    private Dictionary<NetworkIdentity, int> _playerScores = new ();
    
    [Server]
    private void Start()
    {
        scoreEvents.RegisterPlayerEvent += OnRegisterPlayerEvent;
        scoreEvents.UnregisterPlayerEvent += OnUnregisterPlayerEvent;
        hitEvents.HitEvent += AddScore;
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

        if (newScore >= winScores)
        {
            Debug.Log("Player won!");
        }
    }
}
