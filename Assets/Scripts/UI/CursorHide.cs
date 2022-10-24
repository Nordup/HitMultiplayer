using Events;
using UnityEngine;

namespace UI
{
    public class CursorHide : MonoBehaviour
    {
        public NetworkEvents networkEvents;
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError("networkEvents is not set");
            
            networkEvents.ClientConnectEvent += HideCursor;
            networkEvents.ClientDisconnectEvent += ShowCursor;
        }
        
        private void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        private void HideCursor()
        {
            Debug.Log("HideCursor");
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.F)) return;
            
            if (Cursor.visible)
            {
                HideCursor();
            }
            else
            {
                ShowCursor();
            }
        }
        
        private void OnDestroy()
        {
            networkEvents.ClientConnectEvent -= HideCursor;
            networkEvents.ClientDisconnectEvent -= ShowCursor;
        }
    }
}
