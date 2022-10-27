using Events;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MaxPlayers : MonoBehaviour
    {
        public Slider slider;
        public TMP_Text countText;
        public MenuInputEvents menuInputEvents;
        
        private void Start()
        {
            if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
            
            slider.onValueChanged.AddListener(UpdateCount);
            slider.maxValue = NetworkManager.singleton.maxConnections;
            UpdateCount(slider.value);
        }
        
        private void UpdateCount(float count)
        {
            var maxPlayers = (int)count;
            countText.text = maxPlayers.ToString();
            menuInputEvents.MaxPlayersChanged(maxPlayers);
        }
        
        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(UpdateCount);
        }
    }
}