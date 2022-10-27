using HitScriptableObjects;
using UnityEngine;

namespace UI
{
    public class CursorHide : MonoBehaviour
    {
        public NetworkEvents networkEvents;
        
        private bool _isInGame;
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
            
            networkEvents.ClientConnectEvent += OnEnterGame;
            networkEvents.ClientDisconnectEvent += OnLeaveGame;

            _isInGame = false;
        }

        private void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void Update()
        {
            if (!_isInGame) return;
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
        
        private void OnEnterGame()
        {
            _isInGame = true;
            HideCursor();
        }
        
        private void OnLeaveGame()
        {
            _isInGame = false;
            ShowCursor();
        }
        
        private void OnDestroy()
        {
            networkEvents.ClientConnectEvent -= OnEnterGame;
            networkEvents.ClientDisconnectEvent -= OnLeaveGame;
        }
    }
}
