using HitScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerName : MonoBehaviour
    {
        public TMP_InputField playerNameIField;
        public MenuInput menuInput;
        
        private void Start()
        {
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            
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
            menuInput.SetPlayerName(playerName);
        }
        
        private void OnDestroy()
        {
            playerNameIField.onValueChanged.RemoveListener(CheckName);
            playerNameIField.onEndEdit.RemoveListener(SetName);
        }
    }
}