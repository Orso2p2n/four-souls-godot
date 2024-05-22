using Godot;
using Godot.Collections;
using System;

public enum NetworkStatus {
    None,
    Host,
    Client
}

public enum GameState {
    InMenu,
    InLobby,
    InGame
}

public partial class NetworkManager : Node
{
    // Signals
    [Signal] public delegate void ServerCreatedEventHandler();
    [Signal] public delegate void ServerCreationFailedEventHandler();
    [Signal] public delegate void ServerClosedEventHandler();

    [Signal] public delegate void ConnectedToLobbyEventHandler();

    [Signal] public delegate void UserAddedEventHandler(NetworkUser user);
    [Signal] public delegate void UserRemovedEventHandler(NetworkUser user);
    
    // Static
    public static NetworkManager ME;

    // Getters
    public int PeerID {
        get {
            return Multiplayer.GetUniqueId();
        }
    }

    // Variables
    public Array<NetworkUser> Users { get; set; } = new();

    public NetworkStatus Status { get; set; } = NetworkStatus.None;
    public GameState GameState { get; set; } = GameState.InMenu;

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
    private void OnPeerConnected(long id) {
        Console.LogNetwork($"Peer connected {id}.");

        if (Status == NetworkStatus.Host) {
            var inLobby = GameState == GameState.InLobby;
            RpcId(id, MethodName.ReceiveServerLobbyConfirmation, inLobby, Multiplayer.GetPeers());

            if (!inLobby) {
                Console.LogWarning($"Peer {id} tried to join while not in lobby.");
            }
        }
	}

	private void OnConnectedToServer() {
		Console.LogNetwork($"Connected to server: {Address}.");
	}

	private void OnPeerDisconnected(long id) {
		Console.LogNetwork($"Peer disconnected {id}.");

        RemoveUser(id);
	}

	private void OnServerDisconnected() {
		Console.LogWarning($"Disconnected from server: {Address}.");

        Users.Clear();
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
    public void HostUpnp() {
        if (!InitUpnp()) {
            EmitSignal(SignalName.ServerCreationFailed);
            return;
        }

        FinalizeHost();
    }

    public void HostLocal() {
        Address = "127.0.0.1";

        FinalizeHost();
    }

    private void FinalizeHost() {
        var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port, maxClients: 3);
		Multiplayer.MultiplayerPeer = peer;

        // Done
        Console.LogNetwork($"Server open at address {Address}.");
        EmitSignal(SignalName.ServerCreated);
        AddUser(PeerID);
        Status = NetworkStatus.Host;
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

    private bool InitUpnp() {
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

                return true;
            }
            else {
                Console.LogError("Invalid Gateway.");
            }
        }
        else {
            Console.LogError("UPNP Discover failed!");
        }

        return false;
    }

    private void CloseUpnp() {
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
        Status = NetworkStatus.Client;
    }

    
	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void ReceiveServerLobbyConfirmation(bool confirmed, int[] connectedPeers) {
        if (confirmed) {
            AddUser(1);

            foreach (var peer in connectedPeers) {
                AddUser(peer);
            }

            Rpc(MethodName.NewPeerInLobby, PeerID);
            EmitSignal(SignalName.ConnectedToLobby);
        }
        else {
            Disconnect();
        }
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void NewPeerInLobby(int id) {
        AddUser(id);
        Console.LogNetwork($"Peer connected {id}.");
    }

    public void Disconnect() {
        if (Multiplayer.IsServer()) {
            CloseServer();
        }
        else {
            Multiplayer.MultiplayerPeer.DisconnectPeer(1);
        }

        Status = NetworkStatus.None;
    }

    // --- Users ---
    private void AddUser(long id) {
        var isHost = id == 1;
        var isSelf = id == Multiplayer.MultiplayerPeer.GetUniqueId();
        var user = new NetworkUser() { Id = id, IsHost = isHost, IsSelf = isSelf };
        Users.Add(user);

        EmitSignal(SignalName.UserAdded, user);

        Console.LogNetwork($"Network User added with ID {id}.");
    }

    private void RemoveUser(long id) {
        var user = GetUserById(id);

        if (user == null) {
            return;
        }

        Users.Remove(user);

        EmitSignal(SignalName.UserRemoved, user);

        user.QueueFree();

        Console.LogNetwork($"Network User removed with ID {id}.");
    }

    private NetworkUser GetUserById(long id) {
        foreach (var user in Users) {
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
