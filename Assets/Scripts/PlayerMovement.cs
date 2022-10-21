using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed;
    public float mouseSensitivity;
    
    public Transform cameraOrbit;
    public float minAngle;
    public float maxAngle;
    
    private CharacterController _character;
    private float _camRotation;
    
    // Dash
    public float dashTime;
    public float dashDistance;
    
    private float _dashStartTime;
    private Vector3 _dashDirection;
    
    private void Start()
    {
        if (!isLocalPlayer)
        {
            var anotherCamera = GetComponentInChildren<Camera>();
            if (anotherCamera) Destroy(anotherCamera.gameObject);
            return;
        }
        _character = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        if (!isLocalPlayer) return;
        
        HandlePlayerRotation();
        HandlePlayerMovement();
        Dash();
    }
    
    private void HandlePlayerRotation()
    {
        if (Cursor.visible) return;
        
        // Mouse handling
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(transform.up, mouseX);
        
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        _camRotation = Mathf.Clamp(_camRotation + mouseY, minAngle, maxAngle);
        cameraOrbit.localEulerAngles = new Vector3(-_camRotation, 0, 0);
    }
    
    private void HandlePlayerMovement()
    {
        if (_dashDirection != Vector3.zero) return; // Don't move when Dashing
        
        // WASD handling
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = transform.rotation * direction;
        _character.SimpleMove(moveSpeed * direction);
        
        // Dash
        if (Input.GetMouseButtonDown(0))
        {
            _dashDirection = direction.normalized;
            _dashStartTime = Time.time;
        }
    }
    
    private void Dash()
    {
        if (_dashDirection == Vector3.zero) return;
        if (Time.time - _dashStartTime > dashTime)
        {
            _dashDirection = Vector3.zero;
            return;
        }

        var speed = dashDistance / dashTime;
        _character.SimpleMove(  speed * _dashDirection);
    }
}
