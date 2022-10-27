using Events;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PJoinedLeft : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public MenuInputEvents menuInputEvents;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
            if (!isLocalPlayer) return;
            
            CmdInvokePlayerJoined(menuInputEvents.PlayerName);
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