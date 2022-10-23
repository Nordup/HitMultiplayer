using Mirror;
using UnityEngine;

[CreateAssetMenu(menuName = "HitScriptableObjects/HitEvents", fileName = "HitEvents")]
public class HitEvents : ScriptableObject
{
    public delegate void Hit(NetworkIdentity netId);
    public event Hit HitEvent;
    
    public void RaiseHit(NetworkIdentity netId)
    {
        HitEvent?.Invoke(netId);
    }
}