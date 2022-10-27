using System;
using UnityEngine;

namespace HitScriptableObjects
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/MenuInput", fileName = "MenuInput")]
    public class MenuInput : ScriptableObject
    {
        public string PlayerName { get; private set; }
        public string RoomName { get; private set; }
        public int MaxPlayers { get; private set; }
        public Uri SelectedRoomUri { get; private set; }
        
        public void SetPlayerName(string playerName) => PlayerName = playerName;
        
        public void SetRoomName(string roomName) => RoomName = roomName;
        
        public void SetMaxPlayers(int count) => MaxPlayers = count;
        
        public void SetSelectedRoomUri(Uri uri) => SelectedRoomUri = uri;
    }
}