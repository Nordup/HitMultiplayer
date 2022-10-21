using Mirror;
using UnityEngine;

namespace Player
{
    public class Movement : NetworkBehaviour
    {
        // Constants
        public float moveSpeed;
        public float dashTime;
        public float dashDistance;
        
        public bool IsDashing => _dashDirection != Vector3.zero;
        private float _dashStartTime;
        private Vector3 _dashDirection;
        private Vector3 _moveDirection;
        
        private Rigidbody _rigidbody;
        
        private void Start()
        {
            if (!isLocalPlayer) return;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if (!isLocalPlayer) return;
        
            // WASD handling
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.rotation * _moveDirection;
        
            // Dash
            if (Input.GetMouseButtonDown(0) && !IsDashing)
            {
                _dashDirection = _moveDirection.normalized;
                _dashStartTime = Time.time;
            }
        }
        
        private void FixedUpdate()
        {
            if (!isLocalPlayer) return;
        
            if (IsDashing)
            {
                if (Time.time - _dashStartTime > dashTime)
                {
                    _dashDirection = Vector3.zero;
                    return;
                }
                
                var dashSpeed = dashDistance / dashTime;
                var dir = Time.fixedDeltaTime * dashSpeed * _moveDirection;
                _rigidbody.MovePosition(_rigidbody.position + dir);
            }
            else
            {
                var dir = Time.fixedDeltaTime * moveSpeed * _moveDirection;
                _rigidbody.MovePosition(_rigidbody.position + dir);
            }
        }
    }
}
