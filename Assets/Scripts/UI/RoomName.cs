using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RoomName : MonoBehaviour
    {
        public Button createBtn;
        public TMP_InputField roomNameIField;
        public MenuEvents menuEvents;
        
        private void Start()
        {
            if (!menuEvents) Debug.LogError("roomEvents is not set");
            roomNameIField.onValueChanged.AddListener(CheckName);
            CheckName(roomNameIField.text);
        }
        
        private void CheckName(string roomName)
        {
            menuEvents.RoomNameChanged(roomName);
            
            if (string.IsNullOrEmpty(roomName))
                createBtn.interactable = false;
            else if (!createBtn.interactable)
            {
                createBtn.interactable = true;
            }
        }
        
        private void OnDestroy()
        {
            roomNameIField.onValueChanged.RemoveListener(CheckName);
        }
    }
}