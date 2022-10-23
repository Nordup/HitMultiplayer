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
        public RoomEvents roomEvents;
        
        private void Start()
        {
            if (!roomEvents) Debug.LogError("roomEvents is not set");
            roomNameIField.onValueChanged.AddListener(CheckName);
            CheckName(roomNameIField.text);
        }
        
        private void CheckName(string roomName)
        {
            roomEvents.RoomNameChanged(roomName);
            
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