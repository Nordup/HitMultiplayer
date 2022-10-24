using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreateJoinRoom : MonoBehaviour
    {
        public HitNetworkManager manager;
        
        // Create room
        public Button createBtn;
        public Slider maxPlayers;
        
        // Join room
        public Button joinBtn;
        public AvailableRooms availableRooms;
        
        private void Start()
        {
            maxPlayers.maxValue = manager.maxConnections;
            
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
            
            manager.maxConnections = (int)maxPlayers.value;
            manager.StartHost();
        }
        
        private void JoinRoom()
        {
            if (NetworkClient.isConnected || NetworkServer.active)
            {
                Debug.LogError("Cannot join room. Client is connected or server is active");
                return;
            }
            
            manager.StartClient(availableRooms.SelectedRoomUri);
        }
        
        private void OnDestroy()
        {
            createBtn.onClick.RemoveListener(CreateRoom);
            joinBtn.onClick.RemoveListener(JoinRoom);
        }
    }
}
