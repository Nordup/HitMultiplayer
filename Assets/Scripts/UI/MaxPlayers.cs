using HitScriptableObjects;
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
        public MenuInput menuInput;
        
        private void Start()
        {
            if (!menuInput) Debug.LogError($"{nameof(menuInput)} is not set");
            
            slider.onValueChanged.AddListener(UpdateCount);
            slider.maxValue = NetworkManager.singleton.maxConnections;
            UpdateCount(slider.value);
        }
        
        private void UpdateCount(float count)
        {
            var maxPlayers = (int)count;
            countText.text = maxPlayers.ToString();
            menuInput.SetMaxPlayers(maxPlayers);
        }
        
        private void OnDestroy()
        {
            slider.onValueChanged.RemoveListener(UpdateCount);
        }
    }
}