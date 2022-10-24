using System;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/MenuEvents", fileName = "MenuEvents")]
    public class MenuEvents : ScriptableObject
    {
        public event Action<string> PlayerNameChangedEvent;
        public event Action<string> RoomNameChangedEvent;
        public event Action<int> MaxPlayersChangedEvent;
        
        public string PlayerName { get; private set; }
        public string RoomName { get; private set; }
        public int MaxPlayers { get; private set; }
        
        public void PlayerNameChanged(string playerName)
        {
            PlayerName = playerName;
            PlayerNameChangedEvent?.Invoke(playerName);
        }
        
        public void RoomNameChanged(string roomName)
        {
            RoomName = roomName;
            RoomNameChangedEvent?.Invoke(roomName);
        }
        
        public void MaxPlayersChanged(int count)
        {
            MaxPlayers = count;
            MaxPlayersChangedEvent?.Invoke(count);
        }
    }
}