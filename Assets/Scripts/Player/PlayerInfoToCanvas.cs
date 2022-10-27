using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CanvasGroup), typeof(RectTransform))]
    public class PlayerInfoToCanvas : MonoBehaviour
    {
        // Inspector vars
        public Transform lookAt;
        public float scaleMult;
        public float minHeight;
        
        // Components
        private CanvasGroup _canvasGroup;
        private RectTransform _transform;
        
        private Camera _mainCamera;
        private float _minScale;
        
        private void Start()
        {
            _mainCamera = Camera.main;
            _canvasGroup = GetComponent<CanvasGroup>();
            _transform = GetComponent<RectTransform>();
            _minScale = minHeight / _transform.sizeDelta.y;
        }
        
        private void Update()
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
