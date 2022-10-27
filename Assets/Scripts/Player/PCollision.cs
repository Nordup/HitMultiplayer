using System.Threading.Tasks;
using Events;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PMovement))]
    public class PCollision : NetworkBehaviour
    {
        // Inspector vars
        public float hitTime;
        public GameEvents gameEvents;
        public MeshRenderer material;
        public MeshRenderer faceMaterial;
        
        // Components
        private PMovement _pMovement;
        
        [SyncVar(hook = nameof(SetHit))]
        private bool _isHit;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError("gameEvents is not set");
            if (!isServer) return;
            
            _pMovement = GetComponent<PMovement>();
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
            gameEvents.PlayerHit(byPlayer);
            
            await Task.Delay((int)(hitTime * 1000));
            _isHit = false;
        }
        
        [Client]
        private void SetHit(bool oldValue, bool newValue)
        {
            if (!material || !faceMaterial) return;
            var color = _isHit ? Color.red : Color.gray;
            material.material.color = color;
            faceMaterial.material.color = color;
        }
    }
}
