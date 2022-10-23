using Events;
using Mirror;
using UnityEngine;

public class HitNetworkManager : NetworkManager
{
    [Header("HitNetworkManager")]
    public NetworkEvents networkEvents;
    
    public override void OnClientConnect()
    {
        networkEvents.ClientConnect();
        
        NetworkClient.Ready();
        if (NetworkClient.localPlayer == null)
        {
            NetworkClient.AddPlayer();
        }
    }
    
    public override void OnClientDisconnect()
    {
        networkEvents.ClientDisconnect();
    }
}