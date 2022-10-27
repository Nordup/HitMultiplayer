using System;
using System.Net;
using Events;
using Mirror;
using Mirror.Discovery;
using UnityEngine;

[Serializable]
public class DiscoveryRequest : NetworkMessage
{
    public string playerName;
}

[Serializable]
public class DiscoveryResponse : NetworkMessage
{
    public IPEndPoint EndPoint { get; set; }
    public long serverId;
    public Uri Uri;
    
    public string roomName;
    public int totalPlayers;
    public int maxPlayers;
    public bool nameAvailable;
}

public class HitNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
{
    public Transport transport;
    public NetworkEvents networkEvents;
    public MenuInputEvents menuInputEvents;
    
    private long _serverId;

    private void Awake()
    {
        networkEvents.StartDiscoveryEvent += StartDiscovery;
    }

    public override void Start()
    {
        if (!networkEvents) Debug.LogError($"{nameof(networkEvents)} is not set");
        if (!menuInputEvents) Debug.LogError($"{nameof(menuInputEvents)} is not set");
        
        if (transport == null) transport = Transport.activeTransport;
        _serverId = RandomLong();

        base.Start();
    }
    
    // Server part
    
    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
    {
        try
        {
            if (string.IsNullOrEmpty(menuInputEvents.RoomName) || menuInputEvents.MaxPlayers == 0)
                Debug.LogError("Some DiscoveryResponse vars are not set");
            
            return new DiscoveryResponse
            {
                Uri = transport.ServerUri(),
                serverId = _serverId,
                roomName = menuInputEvents.RoomName,
                totalPlayers = NetworkServer.connections.Count,
                maxPlayers = menuInputEvents.MaxPlayers,
                nameAvailable = IsNameAvailable(request.playerName)
            };
        }
        catch (NotImplementedException)
        {
            Debug.LogError($"Transport {transport} does not support network discovery");
            throw;
        }
    }
    
    private bool IsNameAvailable(string playerName)
    {
        // TODO: implement later
        return true;
    }
    
    // Client part
    
    protected override DiscoveryRequest GetRequest()
    {
        if (string.IsNullOrEmpty(menuInputEvents.PlayerName))
            Debug.LogError("Some DiscoveryRequest vars are not set");
        
        return new DiscoveryRequest()
        {
            playerName = menuInputEvents.PlayerName
        };
    }
    
    protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
    {
        response.EndPoint = endpoint;
        
        var realUri = new UriBuilder(response.Uri)
        {
            Host = response.EndPoint.Address.ToString()
        };
        response.Uri = realUri.Uri;
        
        networkEvents.ServerFound(response);
    }
}