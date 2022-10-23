using System;
using System.Collections.Generic;
using System.Linq;
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
        public TMP_Dropdown roomsDropdown;
        public Button refreshBtn;
        public HitNetworkDiscovery networkDiscovery;
        
        [NonSerialized] public Uri SelectedRoomUri;
        
        private readonly Dictionary<long, RoomInfo> _discoveredServers = new ();
        
        private void Start()
        {
            networkDiscovery.ServerFoundEvent += OnServerFound;
            refreshBtn.onClick.AddListener(SearchForRooms);
            roomsDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            SearchForRooms();
        }

        private void OnDropdownValueChanged(int dropdownIndex)
        {
            joinBtn.interactable = true;

            var optionData = roomsDropdown.options[dropdownIndex];
            var roomInfo = _discoveredServers.Values.First(rInfo => rInfo.OptionData == optionData);
            SelectedRoomUri = roomInfo.DResponse.Uri;
        }

        private void SearchForRooms()
        {
            joinBtn.interactable = false;
            
            _discoveredServers.Clear();
            roomsDropdown.ClearOptions();
            
            networkDiscovery.StartDiscovery();
        }

        private void OnServerFound(DiscoveryResponse dResponse)
        {
            if (_discoveredServers.TryGetValue(dResponse.serverId, out var roomInfo))
            {
                // roomInfo.Info = dResponse;
                roomInfo.OptionData.text = $"{dResponse.roomName} {roomInfo.DResponse.totalPlayers++}/{dResponse.maxPlayers}";
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

        private void OnDestroy()
        {
            networkDiscovery.ServerFoundEvent -= OnServerFound;
            refreshBtn.onClick.RemoveListener(SearchForRooms);
            roomsDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        }
    }
}