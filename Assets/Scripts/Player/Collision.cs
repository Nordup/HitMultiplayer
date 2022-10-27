using System.Threading.Tasks;
using HitScriptableObjects;
using Mirror;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Movement))]
    public class Collision : NetworkBehaviour
    {
        // Inspector vars
        public float hitTime;
        public GameEvents gameEvents;
        public MeshRenderer material;
        public MeshRenderer faceMaterial;
        
        // Components
        private Movement _movement;
        
        [SyncVar(hook = nameof(SetHit))]
        private bool _isHit;
        
        private void Start()
        {
            if (!gameEvents) Debug.LogError($"{nameof(gameEvents)} is not set");
            if (!isServer) return;
            
            _movement = GetComponent<Movement>();
        }
        
        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            if (!isServer) return;
            if (!other.gameObject.CompareTag("Player")) return;
            
            var otherPMovement = other.gameObject.GetComponent<Movement>();
            if (!otherPMovement.IsDashing) return;
            
            if (!_movement.IsDashing || otherPMovement.DashStartTime < _movement.DashStartTime)
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
