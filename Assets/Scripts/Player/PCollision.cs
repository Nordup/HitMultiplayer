using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PCollision : NetworkBehaviour
    {
        // Inspector vars
        public float hitTime;
        public HitEvents hitEvents;
        
        // Components
        private PMovement _pMovement;
        private MeshRenderer _meshRenderer;
        
        [SyncVar(hook = nameof(SetHit))]
        private bool _isHit;
        
        private void Start()
        {
            _pMovement = GetComponent<PMovement>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!isServer) return;
            if (!other.gameObject.CompareTag("Player")) return;
            
            var otherPMovement = other.gameObject.GetComponent<PMovement>();
            if (!otherPMovement.IsDashing) return;
            
            if (!_pMovement.IsDashing || otherPMovement.DashStartTime < _pMovement.DashStartTime)
                WasHit(otherPMovement.netIdentity);
        }
        
        [Server]
        private async void WasHit(NetworkIdentity byPlayer)
        {
            if (_isHit) return;
            
            _isHit = true;
            hitEvents.RaiseHit(byPlayer);
            
            await Task.Delay((int)(hitTime * 1000));
            _isHit = false;
        }
        
        [Client]
        private void SetHit(bool oldValue, bool newValue)
        {
            _meshRenderer.material.color = _isHit ? Color.red : Color.gray;
        }
    }
}
