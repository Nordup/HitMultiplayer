using System.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Player
{
    public class PMovement : NetworkBehaviour
    {
        // Inspector vars
        public float moveSpeed;
        public float dashTime;
        public float dashDistance;
        
        // Components
        private Rigidbody _rigidbody;
        
        public bool IsDashing => _dashDirection != Vector3.zero;
        public float DashStartTime => _dashStartTime;
        private Vector3 _dashDirection;
        private float _dashStartTime;
        private Vector3 _moveDirection;
        
        [Client]
        private void Start()
        {
            if (!isLocalPlayer) return;
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        [Client]
        private void Update()
        {
            if (!isLocalPlayer) return;
            
            // WASD handling
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.rotation * _moveDirection;
            if (_moveDirection.magnitude > 1)
                _moveDirection = _moveDirection.normalized;
            
            // Dash
            if (Input.GetMouseButtonDown(0) && !IsDashing && _moveDirection != Vector3.zero)
            {
                _dashDirection = _moveDirection.normalized;
                _dashStartTime = Time.time;
                CmdIamDashing(_dashDirection);
            }
        }
        
        [Client]
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
                var dir = dashSpeed * _dashDirection;
                _rigidbody.velocity = new Vector3(dir.x, _rigidbody.velocity.y, dir.z);
            }
            else
            {
                var dir = moveSpeed * _moveDirection;
                _rigidbody.velocity = new Vector3(dir.x, _rigidbody.velocity.y, dir.z);
            }
        }

        [Command]
        private async void CmdIamDashing(Vector3 direction)
        {
            _dashDirection = direction;
            _dashStartTime = Time.time;
            
             await Task.Delay((int)(dashTime * 1000));
            _dashDirection = Vector3.zero;
        }
    }
}
