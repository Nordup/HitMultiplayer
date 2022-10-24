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
    public MenuEvents menuEvents;
    
    public event Action<DiscoveryResponse> ServerFoundEvent;
    
    private long _serverId;
    
    // Set from RoomEvents
    private string _playerName;
    private string _roomName;
    private int _maxPlayers;

    public override void Start()
    {
        if (!menuEvents) Debug.LogError("roomEvents is not set");
        
        _playerName = menuEvents.PlayerName;
        _roomName = menuEvents.RoomName;
        _maxPlayers = menuEvents.MaxPlayers;
        
        menuEvents.PlayerNameChangedEvent += playerName => { _playerName = playerName; };
        menuEvents.RoomNameChangedEvent += roomName => { _roomName = roomName; };
        menuEvents.MaxPlayersChangedEvent += count => { _maxPlayers = count; };
        
        if (transport == null) transport = Transport.activeTransport;
        _serverId = RandomLong();
        
        base.Start();
    }
    
    // Server part

    protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint) 
    {
        try
        {
            if (string.IsNullOrEmpty(_roomName) || _maxPlayers == 0)
                Debug.LogError("Some DiscoveryResponse vars are not set");
            
            return new DiscoveryResponse
            {
                Uri = transport.ServerUri(),
                serverId = _serverId,
                roomName = _roomName,
                totalPlayers = NetworkServer.connections.Count,
                maxPlayers = _maxPlayers,
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
        if (string.IsNullOrEmpty(_playerName))
            Debug.LogError("Some DiscoveryRequest vars are not set");
        
        return new DiscoveryRequest()
        {
            playerName = _playerName
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
        
        ServerFoundEvent?.Invoke(response);
    }
}