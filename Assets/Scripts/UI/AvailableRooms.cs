using System.Collections.Generic;
using System.Linq;
using HitNetwork;
using HitScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AvailableRooms : MonoBehaviour
    {
        private class RoomInfo
        {
            public TMP_Dropdown.OptionData OptionData;
            public DiscoveryResponse DResponse;
        }
        
        public Button joinBtn;
        public Button refreshBtn;
        public TMP_Dropdown roomsDropdown;
        
        public NetworkEvents networkEvents;
        public MenuInput menuInput;
        
        private readonly Dictionary<long, RoomInfo> _discoveredRooms = new ();
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            
            networkEvents.ServerFoundEvent += OnServerFound;
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
            var roomInfo = _discoveredRooms.Values.First(rInfo => rInfo.OptionData == optionData);
            
            menuInput.SetSelectedRoomUri(roomInfo.DResponse.Uri);
        }

        private void OnServerFound(DiscoveryResponse dResponse)
        {
            if (_discoveredRooms.TryGetValue(dResponse.serverId, out var roomInfo))
            {
                // Update text
                roomInfo.DResponse = dResponse;
                var newText = $"{dResponse.roomName} {roomInfo.DResponse.totalPlayers}/{dResponse.maxPlayers}";
                
                if (roomInfo.OptionData.text == newText) return;
                
                roomInfo.OptionData.text = newText;
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
                    text = $"{dResponse.roomName} {dResponse.totalPlayers}/{dResponse.maxPlayers}"
                };
                _discoveredRooms[dResponse.serverId] = new RoomInfo()
                {
                    OptionData = optionData,
                    DResponse = dResponse
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
            networkEvents.ServerFoundEvent -= OnServerFound;
            roomsDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
            refreshBtn.onClick.RemoveListener(SearchForRooms);
        }
    }
}