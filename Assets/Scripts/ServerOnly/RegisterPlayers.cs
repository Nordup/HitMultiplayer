using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace ServerOnly
{
    public class RegisterPlayers : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public PlayersData playersData;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!playersData) Debug.LogError($"{nameof(playersData)} is not set");
        
            gameEvents.PlayerJoinedEvent += OnPlayerJoined;
            gameEvents.PlayerLeftEvent += OnPlayerLeft;
        }
        
        private void OnPlayerJoined(NetworkIdentity playerId, string playerName)
        {
            playersData.AddPlayer(playerId, playerName);
        }
        
        private void OnPlayerLeft(NetworkIdentity playerId)
        {
            playersData.RemovePlayer(playerId);
        }
        
        private void OnDestroy()
        {
            gameEvents.PlayerJoinedEvent -= OnPlayerJoined;
            gameEvents.PlayerLeftEvent -= OnPlayerLeft;
        }
    }
}