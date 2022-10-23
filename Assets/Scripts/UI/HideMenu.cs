using Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class HideMenu : MonoBehaviour
    {
        public NetworkEvents networkEvents;
        [FormerlySerializedAs("hudPanel")] public GameObject menuPanel;
        
        private void Start()
        {
            if (!networkEvents) Debug.LogError("networkEvents is not set");
            
            networkEvents.ClientConnectEvent += HideHud;
            networkEvents.ClientDisconnectEvent += ShowHud;
        }
        
        private void HideHud()
        {
            menuPanel.SetActive(false);
        }
        
        private void ShowHud()
        {
            menuPanel.SetActive(true);
        }
        
        private void OnDestroy()
        {
            networkEvents.ClientConnectEvent -= HideHud;
            networkEvents.ClientDisconnectEvent -= ShowHud;
        }
    }
}