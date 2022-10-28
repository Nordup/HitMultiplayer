using System;
using System.Net;
using HitScriptableObjects;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

namespace HitNetwork
{
    [Serializable]
    public class DiscoveryRequest : NetworkMessage
    {
        // Nothing for now
    }
    
    [Serializable]
    public class DiscoveryResponse : NetworkMessage
    {
        // public IPEndPoint EndPoint { get; set; }
        public Room room;
    }
    
    public class NetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
    {
        public Transport transport;
        public MenuInput menuInput;
        public NetworkEvents networkEvents;
        
        private long _serverId;
        
        private void Awake()
        {
            networkEvents.StartDiscoveryEvent += StartDiscovery;
        }
        
        public override void Start()
        {
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            
            if (transport == null) transport = Transport.activeTransport;
            _serverId = RandomLong();
            
            base.Start();
        }
        
        // Server part
        
        protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
        {
            try
            {
                if (string.IsNullOrEmpty(menuInput.RoomName) || menuInput.MaxPlayers == 0)
                    Debug.LogError("Some DiscoveryResponse vars are not set");
                
                return new DiscoveryResponse
                {
                    room = new Room()
                    {
                        Uri = transport.ServerUri(),
                        serverId = _serverId,
                        name = menuInput.RoomName,
                        totalPlayers = NetworkServer.connections.Count,
                        maxPlayers = menuInput.MaxPlayers
                    }
                };
            }
            catch (NotImplementedException)
            {
                Debug.LogError($"Transport {transport} does not support network discovery");
                throw;
            }
        }
        
        // Client part
        
        protected override DiscoveryRequest GetRequest()
        {
            return new DiscoveryRequest();
        }
        
        protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
        {
            var room = response.room;
            var realUri = new UriBuilder(room.Uri)
            {
                Host = endpoint.Address.ToString()
            };
            room.Uri = realUri.Uri;
            
            networkEvents.RoomFound(room);
        }
    }
}