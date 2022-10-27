using System;
using UnityEngine;

namespace HitScriptableObjects
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/NetworkEvents", fileName = "NetworkEvents")]
    public class NetworkEvents : ScriptableObject
    {
        public event Action ClientConnectEvent;
        public event Action ClientDisconnectEvent;
        public event Action StartDiscoveryEvent;
        public event Action<DiscoveryResponse> ServerFoundEvent;
        
        public void ClientConnect() => ClientConnectEvent?.Invoke();
        
        public void ClientDisconnect() => ClientDisconnectEvent?.Invoke();

        public void StartDiscovery() => StartDiscoveryEvent?.Invoke();

        public void ServerFound(DiscoveryResponse dResponse) => ServerFoundEvent?.Invoke(dResponse);
    }
}