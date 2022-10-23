using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/RoomEvents", fileName = "RoomEvents")]
    public class RoomEvents : ScriptableObject
    {
        public event Action<string> PlayerNameChangedEvent;
        public event Action<string> RoomNameChangedEvent;
        public event Action<int> MaxPlayersChangedEvent;
        
        public void PlayerNameChanged(string playerName)
        {
            PlayerNameChangedEvent?.Invoke(playerName);
        }
        
        public void RoomNameChanged(string roomName)
        {
            RoomNameChangedEvent?.Invoke(roomName);
        }
        
        public void MaxPlayersChanged(int count)
        {
            MaxPlayersChangedEvent?.Invoke(count);
        }
    }
}