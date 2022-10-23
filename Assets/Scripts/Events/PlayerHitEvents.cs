using System;
using Mirror;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(menuName = "HitScriptableObjects/HitEvents", fileName = "HitEvents")]
    public class PlayerHitEvents : ScriptableObject
    {
        public event Action<NetworkIdentity> HitEvent;
    
        public void PlayerHit(NetworkIdentity netId)
        {
            HitEvent?.Invoke(netId);
        }
    }
}