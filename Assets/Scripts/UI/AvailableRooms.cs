using System.Collections.Generic;
using System.Linq;
using HitScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AvailableRooms : MonoBehaviour
    {
        private class RoomOption
        {
            public TMP_Dropdown.OptionData OptionData;
            public Room Room;
        }
        
        public Button joinBtn;
        public Button refreshBtn;
        public TMP_Dropdown roomsDropdown;
        
        public NetworkEvents networkEvents;
        public MenuInput menuInput;
        
        private readonly Dictionary<long, RoomOption> _discoveredRooms = new ();
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            
            networkEvents.RoomFoundEvent += OnRoomFound;
            roomsDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            refreshBtn.onClick.AddListener(SearchForRooms);
            
            SearchForRooms();
        }
        
        private void SearchForRooms()
        {
            joinBtn.interactable = false;
            
            _discoveredRooms.Clear();
            roomsDropdown.ClearOptions();
            
            networkEvents.StartDiscovery();
        }
        
        private void OnDropdownValueChanged(int dropdownIndex)
        {
            joinBtn.interactable = true;
            
            // Find selected roomInfo
            var optionData = roomsDropdown.options[dropdownIndex];
            var roomOption = _discoveredRooms.Values.First(rOption => rOption.OptionData == optionData);
            
            menuInput.SetSelectedRoomUri(roomOption.Room.Uri);
        }
        
        private void OnRoomFound(Room room)
        {
            if (_discoveredRooms.TryGetValue(room.serverId, out var roomOption))
            {
                // Update text
                roomOption.Room = room;
                var newText = $"{room.name} {room.totalPlayers}/{room.maxPlayers}";
                
                if (roomOption.OptionData.text == newText) return;
                
                roomOption.OptionData.text = newText;
                if (roomsDropdown.IsExpanded)
                {
                    roomsDropdown.enabled = false;
                    roomsDropdown.enabled = true;
                    roomsDropdown.Show();
                }
                
                roomsDropdown.RefreshShownValue();
            }
            else
            {
                // Add new room and option
                var optionData = new TMP_Dropdown.OptionData()
                {
                    text = $"{room.name} {room.totalPlayers}/{room.maxPlayers}"
                };
                _discoveredRooms[room.serverId] = new RoomOption()
                {
                    OptionData = optionData,
                    Room = room
                };
                roomsDropdown.options.Add(optionData);
                roomsDropdown.RefreshShownValue();
                
                if (roomsDropdown.options.Count == 1) OnDropdownValueChanged(0);
            }
        }
        
        private void OnEnable()
        {
            SearchForRooms();
        }
        
        private void OnDestroy()
        {
            networkEvents.RoomFoundEvent -= OnRoomFound;
            roomsDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
            refreshBtn.onClick.RemoveListener(SearchForRooms);
        }
    }
}