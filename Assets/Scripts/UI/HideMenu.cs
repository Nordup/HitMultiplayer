using Events;
using UnityEngine;

namespace UI
{
    public class HideMenu : MonoBehaviour
    {
        public NetworkEvents networkEvents;
        public GameObject menuPanel;
        public GameObject inGameUI;
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            
            networkEvents.ClientConnectEvent += EnterGame;
            networkEvents.ClientDisconnectEvent += LeaveGame;
            LeaveGame();
        }
        
        private void EnterGame()
        {
            inGameUI.SetActive(true);
            menuPanel.SetActive(false);
        }
        
        private void LeaveGame()
        {
            inGameUI.SetActive(false);
            menuPanel.SetActive(true);
        }
        
        private void OnDestroy()
        {
            networkEvents.ClientConnectEvent -= EnterGame;
            networkEvents.ClientDisconnectEvent -= LeaveGame;
        }
    }
}