using Events;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public HitNetworkManager manager;
    public NetworkEvents networkEvents;
    public GameObject hudPanel;
    
    // Create room
    public Button createBtn;
    public TMP_InputField roomName;
    public Slider maxPlayers;
    
    // Join room
    public Button joinBtn;
    public TMP_Dropdown room;
    
    private int _networkManagerMaxConnections;
    
    private void Start()
    {
        if (!networkEvents) Debug.LogError("networkEvents is not set");
        
        maxPlayers.maxValue = manager.maxConnections;
        _networkManagerMaxConnections = manager.maxConnections;
        
        networkEvents.ClientConnectEvent += HideHud;
        networkEvents.ClientDisconnectEvent += ShowHud;
        
        createBtn.onClick.AddListener(CreateRoom);
        joinBtn.onClick.AddListener(JoinRoom);
    }

    private void CreateRoom()
    {
        if (NetworkClient.isConnected || NetworkServer.active)
        {
            Debug.LogError("Cannot create room. Client is connected or server is active");
            return;
        }

        manager.maxConnections = (int)maxPlayers.value;
        manager.StartHost();
    }

    private void JoinRoom()
    {
        if (NetworkClient.isConnected || NetworkServer.active)
        {
            Debug.LogError("Cannot join room. Client is connected or server is active");
            return;
        }
        
        manager.networkAddress = "localhost"; // TODO: get address from dropdown
        manager.StartClient();
    }

    private void HideHud()
    {
        hudPanel.SetActive(false);
    }

    private void ShowHud()
    {
        hudPanel.SetActive(true);
    }

    private void OnDestroy()
    {
        networkEvents.ClientConnectEvent -= HideHud;
        networkEvents.ClientDisconnectEvent -= ShowHud;
        createBtn.onClick.RemoveListener(CreateRoom);
        joinBtn.onClick.RemoveListener(JoinRoom);
    }
    
    // TODO: delete later
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 40, 215, 9999));
        if (NetworkClient.isConnected || NetworkServer.active)
        {
            // host mode
            // display separately because this always confused people:
            //   Server: ...
            //   Client: ...
            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.Label($"<b>Host</b>: running via {Transport.activeTransport}");
            }
            // server only
            else if (NetworkServer.active)
            {
                GUILayout.Label($"<b>Server</b>: running via {Transport.activeTransport}");
            }
            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
            }
        }
        
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Host"))
            {
                manager.StopHost();
            }
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            if (GUILayout.Button("Stop Client"))
            {
                manager.StopClient();
            }
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            if (GUILayout.Button("Stop Server"))
            {
                manager.StopServer();
            }
        }
        GUILayout.EndArea();
    }
}