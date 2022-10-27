using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace Player
{
    public class JoinedLeft : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public MenuInput menuInput;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            if (!isLocalPlayer) return;
            
            CmdInvokePlayerJoined(menuInput.PlayerName);
        }
        
        [Command]
        private void CmdInvokePlayerJoined(string playerName)
        {
            gameEvents.PlayerJoined(netIdentity, playerName);
        }
        
        private void OnDestroy()
        {
            if (!isServer) return;
            gameEvents.PlayerLeft(netIdentity);
        }
    }
}