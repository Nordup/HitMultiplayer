using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PName : NetworkBehaviour
    {
        public MenuEvents menuEvents;
        public TextMeshProUGUI nameText;
        
        [SyncVar(hook = nameof(SetName))]
        private string _playerName;
        
        [Client]
        private void Start()
        {
            if (!menuEvents) Debug.LogError("menuEvents is not set");
            if (!isLocalPlayer) return;
            
            CmdSetName(menuEvents.PlayerName);
        }
        
        [Command] // Sends to server
        private void CmdSetName(string playerName)
        {
            _playerName = playerName;
        }
        
        [Client] // Server syncs with all clients
        private void SetName(string oldName, string newName)
        {
            nameText.text = _playerName;
        }
    }
}