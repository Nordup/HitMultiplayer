using System.Collections.Generic;
using System.Linq;
using Events;
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
        public MenuInputEvents menuInputEvents;
        
        private readonly Dictionary<long, RoomInfo> _discoveredServers = new ();
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
            
            networkEvents.ServerFoundEvent += OnServerFound;
            refreshBtn.onClick.AddListener(SearchForRooms);
            roomsDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            SearchForRooms();
        }

        private void OnDropdownValueChanged(int dropdownIndex)
        {
            joinBtn.interactable = true;

            var optionData = roomsDropdown.options[dropdownIndex];
            var roomInfo = _discoveredServers.Values.First(rInfo => rInfo.OptionData == optionData);
            
            menuInputEvents.SelectedRoomUriChanged(roomInfo.DResponse.Uri);
        }

        private void SearchForRooms()
        {
            joinBtn.interactable = false;
            
            _discoveredServers.Clear();
            roomsDropdown.ClearOptions();
            
            networkEvents.StartDiscovery();
        }

        private void OnServerFound(DiscoveryResponse dResponse)
        {
            if (_discoveredServers.TryGetValue(dResponse.serverId, out var roomInfo))
            {
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
                return;
            }
            
            var optionData = new TMP_Dropdown.OptionData()
            {
                text = $"{dResponse.roomName} {dResponse.totalPlayers}/{dResponse.maxPlayers}"
            };
            _discoveredServers[dResponse.serverId] = new RoomInfo()
            {
                OptionData = optionData,
                DResponse = dResponse
            };
            roomsDropdown.options.Add(optionData);
            roomsDropdown.RefreshShownValue();
            
            if (roomsDropdown.options.Count == 1) OnDropdownValueChanged(0);
        }
        
        private void OnEnable()
        {
            SearchForRooms();
        }
        
        private void OnDestroy()
        {
            networkEvents.ServerFoundEvent -= OnServerFound;
            refreshBtn.onClick.RemoveListener(SearchForRooms);
            roomsDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}