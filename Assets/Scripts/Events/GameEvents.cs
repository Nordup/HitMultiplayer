using System;
using Mirror;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/GameEvents", fileName = "GameEvents")]
    public class GameEvents : ScriptableObject
    {
        public event Action<NetworkIdentity> PlayerWonEvent;
        public event Action ResetScoresEvent;
    
        public void PlayerWon(NetworkIdentity playerId)
        {
            PlayerWonEvent?.Invoke(playerId);
        }
    
        public void ResetScores()
        {
            ResetScoresEvent?.Invoke();
        }
    }
}