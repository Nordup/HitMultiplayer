using Events;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(HitNetworkDiscovery))]
public class HitNetworkManager : NetworkManager
{
    [Header("HitNetworkManager")]
    
    public NetworkEvents networkEvents;
    
    private HitNetworkDiscovery _networkDiscovery;
    private int _totalPlayers;
    
    public override void Start()
    {
        base.Start();
        
        if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
        _networkDiscovery = GetComponent<HitNetworkDiscovery>();
    }
    
    // Server
    
    public override void OnStartServer()
    {
        _networkDiscovery.AdvertiseServer();
    }
    
    public override void OnStopServer()
    {
        _networkDiscovery.StopDiscovery();
    }
    
    // Client
    
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
        NetworkClient.Shutdown();
        networkEvents.ClientDisconnect();
    }
}