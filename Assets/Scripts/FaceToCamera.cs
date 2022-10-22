using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    public float size;
    
    private Transform _cameraTransform;
    private float _cameraFov;
    
    private void Start()
    {
        if (Camera.main == null)
        {
            Debug.LogError("Cannot face to camera. Camera.main is null");
            return;
        }
        _cameraTransform = Camera.main.transform;
        _cameraFov = Mathf.Deg2Rad * Camera.main.fieldOfView;
    }
    
    private void Update()
    {
        if (!_cameraTransform) return;
        
        transform.LookAt(_cameraTransform.position);
        transform.Rotate(0, 180, 0);
        
        var distance = (_cameraTransform.position - transform.position).magnitude;
        var scale = ScaleFactor(distance);
        transform.localScale = scale * Vector3.one;
    }

    private float ScaleFactor(float distance)
    {
        var appealingSize = (2f * Mathf.Atan(size / (2f * distance))) / _cameraFov;
        return 1f / appealingSize;
    }
}