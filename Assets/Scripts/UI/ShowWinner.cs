using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace UI
{
    public class ShowWinner : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public PlayersData playersData;
        public WinnerMenu winnerMenu;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!playersData) Debug.LogError($"{nameof(playersData)} is not set");
            if (!isServer) return;
            
            gameEvents.PlayerWonEvent += OnPlayerWon;
            gameEvents.RestartMatchEvent += OnRestartMatch;
        }
        
        [Server]
        private void OnPlayerWon(NetworkIdentity playerId)
        {
            RpcShowWinnerMenu(playersData.GetName(playerId), playersData.GetScore(playerId));
        }
        
        [Server]
        private void OnRestartMatch()
        {
            RpcHideWinnerMenu();
        }
        
        [ClientRpc]
        private void RpcShowWinnerMenu(string playerName, int score)
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
            gameEvents.RestartMatchEvent -= OnRestartMatch;
        }
    }
}