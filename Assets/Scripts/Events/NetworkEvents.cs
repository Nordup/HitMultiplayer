using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/NetworkEvents", fileName = "NetworkEvents")]
    public class NetworkEvents : ScriptableObject
    {
        public event Action ClientConnectEvent;
        public event Action ClientDisconnectEvent;
        
        public void ClientConnect()
        {
            ClientConnectEvent?.Invoke();
        }
        
        public void ClientDisconnect()
        {
            ClientDisconnectEvent?.Invoke();
        }
    }
}