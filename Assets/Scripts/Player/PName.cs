using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PName : NetworkBehaviour
    {
        public MenuInputEvents menuInputEvents;
        public TextMeshProUGUI nameText;
        
        [SyncVar(hook = nameof(SetName))]
        private string _playerName;
        
        [Client]
        private void Start()
        {
            if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
            if (!isLocalPlayer) return;
            
            CmdSetName(menuInputEvents.PlayerName);
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