using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace HitNetwork
{
    [RequireComponent(typeof(NetworkDiscovery))]
    public class NetworkManager : Mirror.NetworkManager
    {
        public NetworkEvents networkEvents;
        
        private NetworkDiscovery _networkDiscovery;
        
        public override void Start()
        {
            base.Start();
            
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            _networkDiscovery = GetComponent<NetworkDiscovery>();
        }
        
        // Server
        
        public override void OnStartServer()
        {
            _networkDiscovery.AdvertiseServer();
        }
        
        public override void OnStopServer()
        {
            _networkDiscovery.StopDiscovery();
        }
        
        // Client
        
        public override void OnClientConnect()
        {
            networkEvents.ClientConnect();
            
            NetworkClient.Ready();
            if (NetworkClient.localPlayer == null)
            {
                NetworkClient.AddPlayer();
            }
        }
        
        public override void OnClientDisconnect()
        {
            NetworkClient.Shutdown();
            networkEvents.ClientDisconnect();
        }
    }
}