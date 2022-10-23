using Mirror;
using UnityEngine;

[CreateAssetMenu(menuName = "HitScriptableObjects/GameEvents", fileName = "GameEvents")]
public class GameEvents : ScriptableObject
{
    public delegate void PlayerEvent(NetworkIdentity playerId);
    public event PlayerEvent PlayerWonEvent;
    
    public delegate void GameEvent();
    public event GameEvent ResetScoresEvent;
    
    public void PlayerWon(NetworkIdentity playerId)
    {
        PlayerWonEvent?.Invoke(playerId);
    }
    
    public void ResetScores()
    {
        ResetScoresEvent?.Invoke();
    }
}