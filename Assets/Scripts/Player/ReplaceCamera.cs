using Mirror;
using UnityEngine;

namespace Player
{
    public class ReplaceCamera : NetworkBehaviour
    {
        // Inspector vars
        public Transform newParent;
        public Transform newTransform;
        
        private bool _appIsQuitting;
        private Transform _mainCamTransform;
        
        private Transform _savedParent;
        private Vector3 _savedPosition;
        private Quaternion _savedRotation;
        
        public void Start()
        {
            if (!isLocalPlayer) return;
            if (Camera.main == null)
            {
                Debug.LogError("Cannot replace camera. Camera.main is null");
                return;
            }
            
            Application.quitting += () => _appIsQuitting = true;
            _mainCamTransform = Camera.main.transform;
            
            // Save
            _savedParent = _mainCamTransform.parent;
            _savedPosition = _mainCamTransform.position;
            _savedRotation = _mainCamTransform.rotation;
            
            // Change
            _mainCamTransform.SetParent(newParent);
            _mainCamTransform.SetPositionAndRotation(newTransform.position, newTransform.rotation);
        }
        
        public void OnDestroy()
        {
            if (!isLocalPlayer || _appIsQuitting) return;
            
            // Return to previous
            _mainCamTransform.SetParent(_savedParent);
            _mainCamTransform.SetPositionAndRotation(_savedPosition, _savedRotation);
        }
    }
}
