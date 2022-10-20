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
    
    private void Start()
    {
        if (!isLocalPlayer) return;
        _character = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        if (!isLocalPlayer) return;
        
        // WASD handling
        var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        direction = transform.rotation * direction;
        _character.SimpleMove(moveSpeed * direction);
        
        // Mouse handling
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(transform.up, mouseX);
        
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        _camRotation = Mathf.Clamp(_camRotation + mouseY, minAngle, maxAngle);
        cameraOrbit.localEulerAngles = new Vector3(-_camRotation, 0, 0);
    }
}
