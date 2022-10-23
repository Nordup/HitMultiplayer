using Events;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerName : MonoBehaviour
    {
        public TMP_InputField playerNameIField;
        public RoomEvents roomEvents;
        
        private void Start()
        {
            if (!roomEvents) Debug.LogError("roomEvents is not set");
            playerNameIField.onValueChanged.AddListener(CheckName);
            CheckName(playerNameIField.text);
        }
        
        private void CheckName(string playerName)
        {
            // TODO: check name
            roomEvents.PlayerNameChanged(playerName);
        }
        
        private void OnDestroy()
        {
            playerNameIField.onValueChanged.RemoveListener(CheckName);
        }
    }
}