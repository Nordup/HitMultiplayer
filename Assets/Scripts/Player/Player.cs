using Events;
using Mirror;
using UnityEngine;

namespace Player
{
    public class Player : NetworkBehaviour
    {
        public GameEvents gameEvents;
        public MenuEvents menuEvents;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError("gameEvents is not set");
            if (!menuEvents) Debug.LogError("menuEvents is not set");
            
            if (!isLocalPlayer) return;
            CmdInvokePlayerJoined(netIdentity, menuEvents.PlayerName);
        }
        
        [Command]
        private void CmdInvokePlayerJoined(NetworkIdentity playerId, string playerName)
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