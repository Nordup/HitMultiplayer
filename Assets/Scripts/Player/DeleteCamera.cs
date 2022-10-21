using Mirror;
using UnityEngine;

namespace Player
{
    public class DeleteCamera : NetworkBehaviour
    {
        private void Start()
        {
            if (isLocalPlayer) return;
            var anotherCamera = GetComponentInChildren<Camera>();
            if (anotherCamera) Destroy(anotherCamera.gameObject);
        }
    }
}