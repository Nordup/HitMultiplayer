using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class LeaveRoom : MonoBehaviour
    {
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
                NetworkManager.singleton.StopHost();
            }
            else if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
            else if (NetworkServer.active)
            {
                NetworkManager.singleton.StopServer();
            }
        }
        
        private void OnDestroy()
        {
            _leaveRoomBnt.onClick.RemoveListener(OnLeaveRoom);
        }
    }
}
