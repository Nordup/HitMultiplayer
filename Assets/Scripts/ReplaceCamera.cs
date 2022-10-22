using Mirror;
using UnityEngine;

public class ReplaceCamera : NetworkBehaviour
{
    public Transform newParent;
    public Transform newTransform;
    
    private Transform _mainCamTransform;
    
    private Transform _savedParent;
    private Vector3 _savedPosition;
    private Quaternion _savedRotation;
    
    public override void OnStartLocalPlayer()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Cannot replace camera. Camera.main is null");
            return;
        }
        
        _mainCamTransform = Camera.main.transform;
        
        // Save
        _savedParent = _mainCamTransform.parent;
        _savedPosition = _mainCamTransform.position;
        _savedRotation = _mainCamTransform.rotation;
        
        // Change
        _mainCamTransform.SetParent(newParent);
        _mainCamTransform.SetPositionAndRotation(newTransform.position, newTransform.rotation);
    }

    public override void OnStopLocalPlayer()
    {
        // Return to previous
        _mainCamTransform.SetParent(_savedParent);
        _mainCamTransform.SetPositionAndRotation(_savedPosition, _savedRotation);
    }
}
