using HitScriptableObjects;
using Mirror;
using TMPro;
using UnityEngine;

namespace Player
{
    public class Name : NetworkBehaviour
    {
        public MenuInput menuInput;
        public TextMeshProUGUI nameText;
        
        [SyncVar(hook = nameof(SetName))]
        private string _playerName;
        
        [Client]
        private void Start()
        {
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            if (!isLocalPlayer) return;
            
            CmdSetName(menuInput.PlayerName);
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