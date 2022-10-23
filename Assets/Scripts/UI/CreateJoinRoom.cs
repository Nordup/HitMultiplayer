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

        #region Delete Later

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 40, 215, 9999));
            if (NetworkClient.isConnected || NetworkServer.active)
            {
                // host mode
                // display separately because this always confused people:
                //   Server: ...
                //   Client: ...
                if (NetworkServer.active && NetworkClient.active)
                {
                    GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
                }
                // server only
                else if (NetworkServer.active)
                {
                    GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
                }
                // client only
                else if (NetworkClient.isConnected)
                {
                    GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
                }
            }
        
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
            GUILayout.EndArea();
        }

        #endregion
    }
}
