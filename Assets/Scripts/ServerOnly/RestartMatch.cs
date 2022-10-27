using System.Threading.Tasks;
using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace ServerOnly
{
    public class RestartMatch : NetworkBehaviour
    {
        public float matchRestartTime;
        public GameEvents gameEvents;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!isServer) return;
            
            gameEvents.PlayerWonEvent += Restart;
        }
        
        private async void Restart(NetworkIdentity playerI)
        {
            await Task.Delay((int)(matchRestartTime * 1000));
            
            if (!NetworkServer.active) return;
            gameEvents.RestartMatch();
        }
        
        private void OnDestroy()
        {
            if (!isServer) return;
            gameEvents.PlayerWonEvent -= Restart;
        }
        
    }
}