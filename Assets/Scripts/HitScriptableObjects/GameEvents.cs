using System;
using Mirror;
using UnityEngine;

namespace HitScriptableObjects
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/GameEvents", fileName = "GameEvents")]
    public class GameEvents : ScriptableObject
    {
        public event Action<NetworkIdentity, string> PlayerJoinedEvent;
        public event Action<NetworkIdentity> PlayerLeftEvent;
        public event Action<NetworkIdentity> PlayerHitEvent;
        public event Action<NetworkIdentity, int> UpdatePlayerScoreEvent;
        public event Action<NetworkIdentity> PlayerWonEvent;
        public event Action RestartMatchEvent;
        
        public void PlayerJoined(NetworkIdentity playerId, string playerName) => PlayerJoinedEvent?.Invoke(playerId, playerName);
        
        public void PlayerLeft(NetworkIdentity playerId) => PlayerLeftEvent?.Invoke(playerId);
        
        public void PlayerHit(NetworkIdentity netId) =>  PlayerHitEvent?.Invoke(netId);
        
        public void UpdatePlayerScore(NetworkIdentity playerId, int score) => UpdatePlayerScoreEvent?.Invoke(playerId, score);
        
        public void PlayerWon(NetworkIdentity playerId) => PlayerWonEvent?.Invoke(playerId);
        
        public void RestartMatch() => RestartMatchEvent?.Invoke();
    }
}