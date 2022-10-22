using Mirror;
using UnityEngine;

[CreateAssetMenu]
public class HitEvents : ScriptableObject
{
    public delegate void Hit(NetworkIdentity netId);
    public event Hit HitEvent;
    
    public void RaiseHit(NetworkIdentity netId)
    {
        HitEvent?.Invoke(netId);
    }
}