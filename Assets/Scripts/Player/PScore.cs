using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PScore : NetworkBehaviour
    {
        public ScoreEvents scoreEvents;
        public TextMeshPro scoreText;
        
        [SyncVar(hook = nameof(SetScore))]
        private int _score;
        
        public override void OnStartServer()
        {
            if (!scoreEvents) Debug.LogError("scoreEvents is not set");
            
            scoreEvents.RegisterPlayer(netIdentity);
            scoreEvents.UpdateScoreEvent += OnUpdateScore;
        }
        
        public override void OnStopServer()
        {
            scoreEvents.UnregisterPlayer(netIdentity);
            scoreEvents.UpdateScoreEvent -= OnUpdateScore;
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