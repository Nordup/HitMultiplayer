using HitScriptableObjects;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreateJoinRoom : MonoBehaviour
    {
        public Button createBtn;
        public Button joinBtn;
        public MenuInput menuInput;
        
        private void Start()
        {
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            
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
            
            NetworkManager.singleton.maxConnections = menuInput.MaxPlayers;
            NetworkManager.singleton.StartHost();
        }
        
        private void JoinRoom()
        {
            if (NetworkClient.isConnected || NetworkServer.active)
            {
                Debug.LogError("Cannot join room. Client is connected or server is active");
                return;
            }
            
            NetworkManager.singleton.StartClient(menuInput.SelectedRoomUri);
        }
        
        private void OnDestroy()
        {
            createBtn.onClick.RemoveListener(CreateRoom);
            joinBtn.onClick.RemoveListener(JoinRoom);
        }
    }
}
