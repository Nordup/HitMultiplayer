using UnityEngine;

namespace UI
{
    public class ToCanvas : MonoBehaviour
    {
        public Transform lookAt;
        public float scaleMult;
        
        private Camera _mainCamera;
        private CanvasGroup _canvasGroup;
        private Transform _outTransform;

        private Vector3 _scale;
        
        void Start()
        {
            _mainCamera = Camera.main;
            _canvasGroup = GetComponent<CanvasGroup>();
            _outTransform = gameObject.transform;
            _scale = _outTransform.localScale;
        }
        
        void Update()
        {
            var newPos = _mainCamera.WorldToScreenPoint(lookAt.position);
            var distance = (lookAt.position - _mainCamera.gameObject.transform.position).magnitude;
            _outTransform.position = new Vector3(newPos.x, newPos.y, newPos.z);
            _outTransform.localScale = _scale / (distance * scaleMult);
            _canvasGroup.alpha = newPos.z < 0 ? 0 : 1;
        }
    }
}
