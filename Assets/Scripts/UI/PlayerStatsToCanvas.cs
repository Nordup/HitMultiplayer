using UnityEngine;

namespace UI
{
    public class PlayerStatsToCanvas : MonoBehaviour
    {
        public Transform lookAt;
        public float scaleMult;
        public float minHeight;
        
        private Camera _mainCamera;
        private CanvasGroup _canvasGroup;
        private RectTransform _transform;

        private float _minScale;
        
        void Start()
        {
            _mainCamera = Camera.main;
            _canvasGroup = GetComponent<CanvasGroup>();
            _transform = GetComponent<RectTransform>();
            _minScale = minHeight / _transform.sizeDelta.y;
        }
        
        void Update()
        {
            _transform.position = _mainCamera.WorldToScreenPoint(lookAt.position);
            
            // Make smaller if far away
            var distance = (lookAt.position - _mainCamera.gameObject.transform.position).magnitude;
            _transform.localScale = Vector3.one / (distance * scaleMult);
            
            // Don't make smaller than minHeight
            var actualSize = _transform.localScale.y * _transform.sizeDelta.y;
            if (actualSize < minHeight)
            {
                // Uncomment when changing realtime
                // _minScale = minHeight / _transform.sizeDelta.y;
                _transform.localScale = _minScale * Vector3.one;
            }
            
            // Make on top of lookAt.position
            actualSize = _transform.localScale.y * _transform.sizeDelta.y;
            var pos = _transform.position;
            _transform.position = new Vector3(pos.x, pos.y + actualSize / 2, pos.z);
            
            // Hide if besides camera
            _canvasGroup.alpha = _transform.position.z < 0 ? 0 : 1;
        }
    }
}
