using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WinnerMenu : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public PlayersManager playersManager;
        public ScoreManager scoreManager;
        
        public GameObject winnerMenu;
        public TMP_Text playerNameTxt;
        public TMP_Text scoreTxt;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError("gameEvents is not set");
            winnerMenu.SetActive(false);
            
            if (!isServer) return;
            gameEvents.PlayerWonEvent += OnPlayerWon;
            gameEvents.RestartMatchEvent += HideWinnerMenu;
        }
        
        [Server]
        private void OnPlayerWon(NetworkIdentity playerId)
        {
            ShowWinnerMenu(
                playerId,
                playersManager.GetPlayerName(playerId),
                scoreManager.GetPlayerScore(playerId));
        }
        
        [ClientRpc]
        private void ShowWinnerMenu(NetworkIdentity playerId, string playerName, int score)
        {
            playerNameTxt.text = playerName;
            scoreTxt.text = score.ToString();
            winnerMenu.SetActive(true);
        }
        
        [ClientRpc]
        private void HideWinnerMenu()
        {
            winnerMenu.SetActive(false);
        }
        
        private void OnDestroy()
        {
            if (!isServer) return;
            gameEvents.PlayerWonEvent -= OnPlayerWon;
            gameEvents.RestartMatchEvent -= HideWinnerMenu;
        }
    }
}