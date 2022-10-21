using Mirror;
using UnityEngine;

namespace Player
{
    public class Collision : NetworkBehaviour
    {
        private Movement _movement;
        
        private void Start()
        {
            _movement = GetComponent<Movement>();
        }
        
        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (!isLocalPlayer) return;
            
            if (other.gameObject.CompareTag("Player"))
            {
                print(other.gameObject.name);
            }
        }
    }
}
