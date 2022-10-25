using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class LeaveRoom : MonoBehaviour
    {
        public HitNetworkManager manager;
        
        private Button _leaveRoomBnt;
        
        private void Start()
        {
            _leaveRoomBnt = GetComponent<Button>();
            _leaveRoomBnt.onClick.AddListener(OnLeaveRoom);
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
            _leaveRoomBnt.onClick.RemoveListener(OnLeaveRoom);
        }
    }
}
