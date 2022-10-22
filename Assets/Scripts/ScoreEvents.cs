using Mirror;
using UnityEngine;

[CreateAssetMenu]
public class ScoreEvents : ScriptableObject
{
    public delegate void RegisterEvent(NetworkIdentity playerId);
    public event RegisterEvent RegisterPlayerEvent;
    public event RegisterEvent UnregisterPlayerEvent;
    
    public delegate void ScoreEvent(NetworkIdentity playerId, int score);
    public event ScoreEvent UpdateScoreEvent;
    
    public void RegisterPlayer(NetworkIdentity playerId)
    {
        RegisterPlayerEvent?.Invoke(playerId);
    }
    
    public void UnregisterPlayer(NetworkIdentity playerId)
    {
        UnregisterPlayerEvent?.Invoke(playerId);
    }

    public void UpdateScore(NetworkIdentity playerId, int score)
    {
        UpdateScoreEvent?.Invoke(playerId, score);
    }
}