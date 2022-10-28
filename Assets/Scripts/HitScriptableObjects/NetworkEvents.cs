using System;
using UnityEngine;

namespace HitScriptableObjects
{
    [Serializable]
    public class Room
    {
        public long serverId;
        public Uri Uri;
        
        public string name;
        public int totalPlayers;
        public int maxPlayers;
    }
    
    [CreateAssetMenu(menuName = "HitScriptableObjects/NetworkEvents", fileName = "NetworkEvents")]
    public class NetworkEvents : ScriptableObject
    {
        public event Action ClientConnectEvent;
        public event Action ClientDisconnectEvent;
        public event Action StartDiscoveryEvent;
        public event Action<Room> RoomFoundEvent;
        
        public void ClientConnect() => ClientConnectEvent?.Invoke();
        
        public void ClientDisconnect() => ClientDisconnectEvent?.Invoke();
        
        public void StartDiscovery() => StartDiscoveryEvent?.Invoke();
        
        public void RoomFound(Room room) => RoomFoundEvent?.Invoke(room);
    }
}