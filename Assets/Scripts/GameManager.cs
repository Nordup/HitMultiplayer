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
    public WinnerMenu winnerMenu;
    
    private void Start()
    {
        if (!gameEvents) Debug.LogError("gameEvents is not set");
        gameEvents.PlayerWonEvent += OnPlayerWon;
    }
    
    [Server]
    private async void OnPlayerWon(NetworkIdentity playerId)
    {
        ShowWinnerMenu(
            playerId,
            playersManager.GetPlayerName(playerId),
            scoreManager.GetPlayerScore(playerId));
        
        await Task.Delay((int)(matchRestartTime * 1000));

        HideWinnerMenu();
        gameEvents.RestartMatch();
    }
    
    [ClientRpc]
    private void ShowWinnerMenu(NetworkIdentity playerId, string playerName, int score)
    {
        winnerMenu.ShowMenu(playerName, score);
    }
    
    [ClientRpc]
    private void HideWinnerMenu()
    {
        winnerMenu.HideMenu();
    }
    
    private void OnDestroy()
    {
        gameEvents.PlayerWonEvent -= OnPlayerWon;
    }
}
