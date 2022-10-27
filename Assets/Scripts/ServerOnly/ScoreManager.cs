using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace ServerOnly
{
    public class ScoreManager : NetworkBehaviour
    {
        public int winScores;
        public GameEvents gameEvents;
        public PlayersData playersData;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!playersData) Debug.LogError($"{nameof(playersData)} is not set");
            
            gameEvents.PlayerHitEvent += AddScore;
            gameEvents.RestartMatchEvent += ClearScores;
        }
        
        private void AddScore(NetworkIdentity playerId)
        {
            var newScore = playersData.GetScore(playerId) + 1;
            playersData.SetScore(playerId, newScore);
            gameEvents.UpdatePlayerScore(playerId, newScore);
            
            if (newScore >= winScores) gameEvents.PlayerWon(playerId);
        }
        
        private void ClearScores()
        {
            foreach (var playerId in playersData.GetPlayers())
            {
                playersData.SetScore(playerId, 0);
                gameEvents.UpdatePlayerScore(playerId, 0);
            }
        }
        
        private void OnDestroy()
        {
            gameEvents.PlayerHitEvent -= AddScore;
            gameEvents.RestartMatchEvent -= ClearScores;
        }
    }
}