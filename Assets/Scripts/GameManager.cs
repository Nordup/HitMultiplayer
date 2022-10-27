using System.Threading.Tasks;
using Events;
using Mirror;
using UI;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public float matchRestartTime;
    public GameEvents gameEvents;
    public PlayersManager playersManager;
    public ScoreManager scoreManager;
    
    // UI calls on clients
    public WinnerMenu winnerMenu;
    
    private void Start()
    {
        if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
        if (!isServer) return;
        gameEvents.PlayerWonEvent += OnPlayerWon;
    }
    
    [Server]
    private async void OnPlayerWon(NetworkIdentity playerId)
    {
        RpcShowWinnerMenu(
            playerId,
            playersManager.GetPlayerName(playerId),
            scoreManager.GetPlayerScore(playerId));
        
        await Task.Delay((int)(matchRestartTime * 1000));
        
        if (!isServer) return;
        RpcHideWinnerMenu();
        gameEvents.RestartMatch();
    }
    
    [ClientRpc]
    private void RpcShowWinnerMenu(NetworkIdentity playerId, string playerName, int score)
    {
        winnerMenu.ShowMenu(playerName, score);
    }
    
    [ClientRpc]
    private void RpcHideWinnerMenu()
    {
        winnerMenu.HideMenu();
    }
    
    private void OnDestroy()
    {
        if (!isServer) return;
        gameEvents.PlayerWonEvent -= OnPlayerWon;
    }
}
