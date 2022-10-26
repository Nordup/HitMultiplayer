using TMPro;
using UnityEngine;

namespace UI
{
    public class WinnerMenu : MonoBehaviour
    {
        public GameObject winnerMenu;
        public TMP_Text playerNameTxt;
        public TMP_Text scoreTxt;
        
        private void Start()
        {
            winnerMenu.SetActive(false);
        }
        
        public void ShowMenu(string playerName, int score)
        {
            playerNameTxt.text = playerName;
            scoreTxt.text = score.ToString();
            winnerMenu.SetActive(true);
        }
        
        public void HideMenu()
        {
            winnerMenu.SetActive(false);
        }
        
        public void OnDisable()
        {
            HideMenu();
        }
    }
}