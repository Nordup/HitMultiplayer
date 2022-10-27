using Events;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreateJoinRoom : MonoBehaviour
    {
        public Button createBtn;
        public Button joinBtn;
        public MenuInputEvents menuInputEvents;
        
        private void Start()
        {
            if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
            
            createBtn.onClick.AddListener(CreateRoom);
            joinBtn.onClick.AddListener(JoinRoom);
        }
        
        private void CreateRoom()
        {
            if (NetworkClient.isConnected || NetworkServer.active)
            {
                Debug.LogError("Cannot create room. Client is connected or server is active");
                return;
            }
            
            NetworkManager.singleton.maxConnections = menuInputEvents.MaxPlayers;
            NetworkManager.singleton.StartHost();
        }
        
        private void JoinRoom()
        {
            if (NetworkClient.isConnected || NetworkServer.active)
            {
                Debug.LogError("Cannot join room. Client is connected or server is active");
                return;
            }
            
            NetworkManager.singleton.StartClient(menuInputEvents.SelectedRoomUri);
        }
        
        private void OnDestroy()
        {
            createBtn.onClick.RemoveListener(CreateRoom);
            joinBtn.onClick.RemoveListener(JoinRoom);
        }
    }
}
