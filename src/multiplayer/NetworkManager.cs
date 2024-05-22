using Godot;
using Godot.Collections;
using System;

public partial class NetworkManager : Node
{
    // Signals
    [Signal] public delegate void ServerCreatedEventHandler();
    [Signal] public delegate void ServerClosedEventHandler();
    [Signal] public delegate void UserAddedEventHandler(NetworkUser user);
    [Signal] public delegate void UserRemovedEventHandler(NetworkUser user);
    
    // Static
    public static NetworkManager ME;

    // Variables
    public Array<NetworkUser> users = new();

    public string Address { get; set; } = "";
    public const int Port = 45075;

    private Upnp upnp;

    public override void _EnterTree() {
        ME = this;

        // Connect Signals
        Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.ConnectedToServer += OnConnectedToServer;
		Multiplayer.PeerDisconnected += OnPeerDisconnected;
		Multiplayer.ServerDisconnected += OnServerDisconnected;
    }

    // --- Connected signals ---
    void OnPeerConnected(long id) {
		Console.LogNetwork($"Peer connected ID: {id}.");

        AddUser(id);
	}

	void OnConnectedToServer() {
		Console.LogNetwork($"Connected to server: {Address}.");
	}

	void OnPeerDisconnected(long id) {
		Console.LogNetwork($"Peer disconnected ID: {id}.");

        RemoveUser(id);
	}

	void OnServerDisconnected() {
		Console.LogWarning($"Disconnected from server: {Address}.");

        users.Clear();
	}

    // --- General ---
    public void SetAddress(string newAddress) {
        Address = newAddress;
    }

    public override void _Notification(int what) {
		base._Notification(what);
		
		if (what == NotificationWMCloseRequest) {
			CloseUpnp();
		}
	}

    // --- Host ---
    public void Host() {
        InitUpnp();

        var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = peer;

        // Done
        EmitSignal(SignalName.ServerCreated);
        AddUser(Multiplayer.MultiplayerPeer.GetUniqueId());
    }

    public void CloseServer() {
        var peers = Multiplayer.GetPeers();

        foreach (var peer in peers) {
            Multiplayer.MultiplayerPeer.DisconnectPeer(peer);
        }

        Multiplayer.MultiplayerPeer.Close();

        CloseUpnp();

        Console.LogWarning($"Server closed.");

        EmitSignal(SignalName.ServerClosed);
    }

    void InitUpnp() {
        Console.LogNetwork("Initializing UPNP...");

        upnp = new Upnp();
        var discoverResult = upnp.Discover();

        Console.LogNetwork("UPNP Discovering...");

        if (discoverResult == (int) Upnp.UpnpResult.Success) {
            Console.LogNetwork("UPNP Discover successful!");

            if (upnp.GetGateway() != null && upnp.GetGateway().IsValidGateway()) {
                upnp.AddPortMapping(Port, Port, (string) ProjectSettings.GetSetting("application/config/name"), "UDP");
                upnp.AddPortMapping(Port, Port, (string) ProjectSettings.GetSetting("application/config/name"), "TCP");

                Address = upnp.QueryExternalAddress();

                Console.LogNetwork($"Server open at address {Address}.");
            }
            else {
                Console.LogError("Invalid Gateway.");
            }
        }
        else {
            Console.LogError("UPNP Discover failed!");
        }
    }

    void CloseUpnp() {
        if (upnp == null) {
            return;
        }

        upnp.DeletePortMapping(Port, "UDP");
        upnp.DeletePortMapping(Port, "TCP");
    }

    // --- Client ---
    public void Connect() {
        if (Address == "") {
            Console.LogError($"Address is empty.");
            return;
        }

        var peer = new ENetMultiplayerPeer();
		var err = peer.CreateClient(Address, Port);
		if (err != Godot.Error.Ok) {
			Console.LogError($"Unable to connect to server at address {Address}.");
			return;
		}

		Multiplayer.MultiplayerPeer = peer;

        // Done
        AddUser(Multiplayer.MultiplayerPeer.GetUniqueId());
    }

    public void Disconnect() {
        if (Multiplayer.IsServer()) {
            CloseServer();
        }
        else {
            Multiplayer.MultiplayerPeer.DisconnectPeer(1);
        }
    }

    // --- Users ---
    void AddUser(long id) {
        var isHost = id == 1;
        var isSelf = id == Multiplayer.MultiplayerPeer.GetUniqueId();
        var user = new NetworkUser() { Id = id, IsHost = isHost, IsSelf = isSelf };
        users.Add(user);

        EmitSignal(SignalName.UserAdded, user);

        Console.LogNetwork($"Network User added with ID {id}.");
    }

    void RemoveUser(long id) {
        var user = GetUserById(id);
        users.Remove(user);

        EmitSignal(SignalName.UserRemoved, user);

        user.QueueFree();

        Console.LogNetwork($"Network User removed with ID {id}.");
    }

    NetworkUser GetUserById(long id) {
        foreach (var user in users) {
            if (user.Id == id) {
                return user;
            }
        }

        return null;
    }

    public void SetUsernameForId(long id, string username) {
        var user = GetUserById(id);
        user?.SetUsername(username);
    }
}
