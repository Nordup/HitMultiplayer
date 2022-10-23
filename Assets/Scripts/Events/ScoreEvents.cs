using System;
using Mirror;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/ScoreEvents", fileName = "ScoreEvents")]
    public class ScoreEvents : ScriptableObject
    {
        public event Action<NetworkIdentity> RegisterPlayerEvent;
        public event Action<NetworkIdentity> UnregisterPlayerEvent;
        public event Action<NetworkIdentity, int> UpdateScoreEvent;
    
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
}