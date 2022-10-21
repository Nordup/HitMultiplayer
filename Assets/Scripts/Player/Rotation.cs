using Mirror;
using UnityEngine;

namespace Player
{
    public class Rotation : NetworkBehaviour
    {
        public float mouseSensitivity;
        
        [Header("Camera")]
        public Transform cameraOrbit;
        public float minAngle;
        public float maxAngle;
        
        private float _camRotation;
        
        private void Update()
        {
            if (!isLocalPlayer) return;
            if (Cursor.visible) return;
            
            // Mouse handling
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.Rotate(transform.up, mouseX);
            
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
            _camRotation = Mathf.Clamp(_camRotation + mouseY, minAngle, maxAngle);
            cameraOrbit.localEulerAngles = new Vector3(-_camRotation, 0, 0);
        }
    }
}
