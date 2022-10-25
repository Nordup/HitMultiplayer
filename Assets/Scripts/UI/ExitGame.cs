using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ExitGame : MonoBehaviour
    {
        private Button _exitGameBtn;
        
        void Start()
        {
            _exitGameBtn = GetComponent<Button>();
            _exitGameBtn.onClick.AddListener(OnExitGame);
        }
        
        private void OnExitGame()
        {
            Debug.Log("Quit ignored in the Editor.");
            Application.Quit();
        }
        
        private void OnDestroy()
        {
            _exitGameBtn.onClick.RemoveListener(OnExitGame);
        }
    }
}
