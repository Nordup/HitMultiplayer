using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MaxPlayers : MonoBehaviour
    {
        public Slider slider;
        public TMP_Text countText;
        public RoomEvents roomEvents;
        
        private void Start()
        {
            if (!roomEvents) Debug.LogError("roomEvents is not set");
            slider.onValueChanged.AddListener(UpdateCount);
            UpdateCount(slider.value);
        }
        
        private void UpdateCount(float count)
        {
            var maxPlayers = (int)count;
            countText.text = maxPlayers.ToString();
            roomEvents.MaxPlayersChanged(maxPlayers);
        }
        
        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(UpdateCount);
        }
    }
}