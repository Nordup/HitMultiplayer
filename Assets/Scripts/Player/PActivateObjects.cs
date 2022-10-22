using Mirror;
using UnityEngine;

namespace Player
{
    public class PActivateObjects : NetworkBehaviour
    {
        public GameObject[] gObjects;
        
        private void Start()
        {
            if (!isLocalPlayer) return;
            foreach (var gObject in gObjects)
            {
                gObject.SetActive(true);
            }
        }
    }
}