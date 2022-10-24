using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaveRoom : MonoBehaviour
    {
        public HitNetworkManager manager;
        public Button leaveRoomBnt;
        
        void Start()
        {
            leaveRoomBnt.onClick.AddListener(OnLeaveRoom);
        }

        private void OnLeaveRoom()
        {
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                manager.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                manager.StopClient();
            }
            else if (NetworkServer.active)
            {
                manager.StopServer();
            }
        }

        private void OnDestroy()
        {
            leaveRoomBnt.onClick.RemoveListener(OnLeaveRoom);
        }
    }
}
