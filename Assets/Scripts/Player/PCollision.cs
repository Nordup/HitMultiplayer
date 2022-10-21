using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PCollision : NetworkBehaviour
    {
        // Inspector vars
        public float hitTime;
        
        private bool _isHit;
        
        private PMovement _pMovement;
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _pMovement = GetComponent<PMovement>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!isServer) return;
            if (!other.gameObject.CompareTag("Player")) return;
            
            var otherPlayer = other.gameObject.GetComponent<PCollision>();
            if (_pMovement.IsDashing) otherPlayer.TargetOnHit();
        }
        
        [TargetRpc]
        private void TargetOnHit()
        {
            if (_isHit) return;
            Debug.Log("Hit by enemy");

            _isHit = true;
            _meshRenderer.material.color = Color.red;
            UnHit();
        }

        private async void UnHit()
        {
            await Task.Delay((int)(hitTime * 1000));
            Debug.Log("Hit timed out");
            
            _isHit = false;
            _meshRenderer.material.color = Color.gray;
        }
    }
}
