using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PScore : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public TextMeshProUGUI scoreText;
        
        [SyncVar(hook = nameof(SetScore))]
        private int _score;
        
        public override void OnStartServer()
        {
            if (!gameEvents) Debug.LogError("gameEvents is not set");
            gameEvents.UpdatePlayerScoreEvent += OnUpdateScore;
        }
        
        public override void OnStopServer()
        {
            gameEvents.UpdatePlayerScoreEvent -= OnUpdateScore;
        }
        
        [Server]
        private void OnUpdateScore(NetworkIdentity playerId, int score)
        {
            if (playerId != netIdentity) return;

            _score = score;
        }
        
        [Client]
        private void SetScore(int oldScore, int newScore)
        {
            scoreText.text = _score.ToString();
        }
    }
}