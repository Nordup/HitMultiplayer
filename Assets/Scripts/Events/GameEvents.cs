using System;
using Mirror;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/GameEvents", fileName = "GameEvents")]
    public class GameEvents : ScriptableObject
    {
        public event Action<NetworkIdentity, string> PlayerJoinedEvent;
        public void PlayerJoined(NetworkIdentity playerId, string playerName)
            => PlayerJoinedEvent?.Invoke(playerId, playerName);
        
        public event Action<NetworkIdentity> PlayerLeftEvent;
        public void PlayerLeft(NetworkIdentity playerId)
            => PlayerLeftEvent?.Invoke(playerId);
        
        public event Action<NetworkIdentity> PlayerHitEvent;
        public void PlayerHit(NetworkIdentity netId)
            =>  PlayerHitEvent?.Invoke(netId);
        
        public event Action<NetworkIdentity, int> UpdatePlayerScoreEvent;
        public void UpdatePlayerScore(NetworkIdentity playerId, int score)
            => UpdatePlayerScoreEvent?.Invoke(playerId, score);
        
        public event Action<NetworkIdentity> PlayerWonEvent;
        public void PlayerWon(NetworkIdentity playerId)
            => PlayerWonEvent?.Invoke(playerId);
        
        public event Action RestartMatchEvent;
        public void RestartMatch()
            => RestartMatchEvent?.Invoke();
    }
}