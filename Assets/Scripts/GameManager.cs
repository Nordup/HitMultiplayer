using System;
using System.Threading.Tasks;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public float matchResetTime;
    public GameEvents gameEvents;
    
    [Server]
    private void Start()
    {
        gameEvents.PlayerWonEvent += OnPlayerWon;
    }
    
    private async void OnPlayerWon(NetworkIdentity playerId)
    {
        await Task.Delay(((int)matchResetTime * 1000));
        gameEvents.ResetScores();
    }
}
