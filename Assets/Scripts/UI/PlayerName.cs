using Events;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class PlayerName : MonoBehaviour
    {
        public TMP_InputField playerNameIField;
        public MenuEvents menuEvents;
        
        private void Start()
        {
            if (!menuEvents) Debug.LogError("roomEvents is not set");
            playerNameIField.onValueChanged.AddListener(CheckName);
            CheckName(playerNameIField.text);
        }
        
        private void CheckName(string playerName)
        {
            if (!string.IsNullOrEmpty(playerName)) menuEvents.PlayerNameChanged(playerName);
        }
        
        private void OnDestroy()
        {
            playerNameIField.onValueChanged.RemoveListener(CheckName);
        }
    }
}