using Events;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PName : NetworkBehaviour
    {
        public MenuEvents menuEvents;
        public TextMeshPro nameText;
        
        [SyncVar(hook = nameof(SetName))]
        private string _playerName;
        
        [Client] // Local client sets his name on server
        private void Start()
        {
            if (!isLocalPlayer) return;
            if (!menuEvents) Debug.LogError("roomEvents is not set");
            CmdSetName(menuEvents.PlayerName); // Shouldn't be changed during game
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