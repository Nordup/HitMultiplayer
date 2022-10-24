using UnityEngine;

namespace UI
{
    public class ToCanvas : MonoBehaviour
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
            var newPos = _mainCamera.WorldToScreenPoint(lookAt.position);
            _transform.position = new Vector3(newPos.x, newPos.y, newPos.z);
            
            var distance = (lookAt.position - _mainCamera.gameObject.transform.position).magnitude;
            _transform.localScale = Vector3.one / (distance * scaleMult);
            
            var actualSize = _transform.localScale.y * _transform.sizeDelta.y;
            if (actualSize < minHeight)
            {
                _minScale = minHeight / _transform.sizeDelta.y;
                _transform.localScale = _minScale * Vector3.one;
            }

            actualSize = _transform.localScale.y * _transform.sizeDelta.y;
            var pos = _transform.position;
            _transform.position = new Vector3(pos.x, pos.y + actualSize / 2, pos.z);
            
            _canvasGroup.alpha = newPos.z < 0 ? 0 : 1;
        }
    }
}
