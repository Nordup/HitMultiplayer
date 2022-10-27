using Events;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerName : MonoBehaviour
    {
        public TMP_InputField playerNameIField;
        public MenuInputEvents menuInputEvents;
        
        private void Start()
        {
            if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
            
            playerNameIField.onValueChanged.AddListener(CheckName);
            playerNameIField.onEndEdit.AddListener(SetName);
            
            SetName(playerNameIField.text);
        }
        
        private void CheckName(string playerName)
        {
            // TODO: disable create and join
        }
        
        private void SetName(string playerName)
        {
            menuInputEvents.PlayerNameChanged(playerName);
        }
        
        private void OnDestroy()
        {
            playerNameIField.onValueChanged.RemoveListener(CheckName);
            playerNameIField.onEndEdit.RemoveListener(SetName);
        }
    }
}