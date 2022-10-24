using System.Threading.Tasks;
using Events;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public float matchRestartTime;
    public GameEvents gameEvents;
    
    [Server]
    private void Start()
    {
        if (!gameEvents) Debug.LogError("gameEvents is not set");
        gameEvents.PlayerWonEvent += OnPlayerWon;
    }
    
    [Server]
    private async void OnPlayerWon(NetworkIdentity playerId)
    {
        await Task.Delay((int)(matchRestartTime * 1000));
        gameEvents.RestartMatch();
    }
    
    private void OnDestroy()
    {
        gameEvents.PlayerWonEvent -= OnPlayerWon;
    }
}
