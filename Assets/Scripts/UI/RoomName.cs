using HitScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RoomName : MonoBehaviour
    {
        public Button createBtn;
        public TMP_InputField roomNameIField;
        public MenuInput menuInput;
        
        private void Start()
        {
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            
            roomNameIField.onValueChanged.AddListener(CheckName);
            roomNameIField.onEndEdit.AddListener(SetName);
            
            SetName(roomNameIField.text);
        }
        
        private void CheckName(string roomName)
        {
            if (string.IsNullOrEmpty(roomName))
                createBtn.interactable = false;
            else if (!createBtn.interactable)
            {
                createBtn.interactable = true;
            }
        }

        private void SetName(string roomName)
        {
            menuInput.SetRoomName(roomName);
        }
        
        private void OnDestroy()
        {
            roomNameIField.onValueChanged.RemoveListener(CheckName);
            roomNameIField.onEndEdit.RemoveListener(SetName);
        }
    }
}