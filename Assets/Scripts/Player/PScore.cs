using Mirror;
using UnityEngine;

namespace Player
{
    public class PScore : NetworkBehaviour
    {
        public ScoreEvents scoreEvents;
        
        private int _score;
        
        public override void OnStartServer()
        {
            scoreEvents.RegisterPlayer(netIdentity);
            scoreEvents.UpdateScoreEvent += OnUpdateScore;
        }
        
        [Server]
        private void OnUpdateScore(NetworkIdentity playerId, int score)
        {
            if (playerId != netIdentity) return;

            _score = score;
            Debug.Log($"{gameObject.name} score is {score}");
        }
        
        public override void OnStopServer()
        {
            scoreEvents.UnregisterPlayer(netIdentity);
        }
    }
}