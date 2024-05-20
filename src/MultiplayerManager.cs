using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;


public partial class MultiplayerManager : Node
{
	[Export] private Control _menu;
	[Export] private RichTextLabel _printLabel;
	[Export] private Label _infoLabel;
	[Export] private TextEdit _textEdit;
	[Export] private Control _ipContainer;
	[Export] private TextEdit _ipTextEdit;

	string _address = "";
	const int Port = 45075;

	bool _connected = false;
	bool _entering_ip = false;

	Upnp upnp;

    public override void _Ready() {
		_textEdit.Hide();
		_ipContainer.Hide();

		Multiplayer.PeerConnected += OnPeerConnected;
		Multiplayer.ConnectedToServer += OnConnectedToServer;
		Multiplayer.PeerDisconnected += OnPeerDisconnected;
		Multiplayer.ServerDisconnected += OnServerDisconnected;
    }

    void OnHostPressed() {
		_menu.Hide();

		InitUpnp();

		_connected = true;
		_textEdit.Show();

		// Create server
        var peer = new ENetMultiplayerPeer();
		peer.CreateServer(Port);
		Multiplayer.MultiplayerPeer = peer;
		
		_infoLabel.Text = "Server";
	}
	
	void InitUpnp() {
		Print("Initializing UPNP...");

		upnp = new Upnp();
		var discoverResult = upnp.Discover();

		Print("UPNP Discovering...");

		if (discoverResult == (int) Upnp.UpnpResult.Success) {
			Print("UPNP Discover successful!");

			if (upnp.GetGateway() != null && upnp.GetGateway().IsValidGateway()) {
				upnp.AddPortMapping(Port, Port, (string) ProjectSettings.GetSetting("application/config/name"), "UDP");
				upnp.AddPortMapping(Port, Port, (string) ProjectSettings.GetSetting("application/config/name"), "TCP");
				
				_address = upnp.QueryExternalAddress();

				Print($"Server open at address {_address}");
			}
			else {
				Print("Invalid Gateway.");
			}
		}
		else {
			Print("UPNP Discover failed!");
		}
	}

	void CloseUpnp() {
		if (upnp == null) {
			return;
		}

		upnp.DeletePortMapping(Port, "UDP");
		upnp.DeletePortMapping(Port, "TCP");
	}

	void OnJoinPressed() {
		_menu.Hide();

		_ipContainer.Show();

		_entering_ip = true;
	}

	void ConnectClient() {
		_connected = true;
		_ipContainer.Hide();
		_textEdit.Show();

		// Create client
        var peer = new ENetMultiplayerPeer();
		var err = peer.CreateClient(_address, Port);
		if (err != Godot.Error.Ok) {
			Print($"Unable to connect to server at address {_address}");
			return;
		}

		Multiplayer.MultiplayerPeer = peer;

		_infoLabel.Text = $"Client ID: {Multiplayer.GetUniqueId()}";
	}
	
	void OnPeerConnected(long id) {
		Print($"Peer connected ID: {id}");
	}

	void OnConnectedToServer() {
		Print($"Connected to server: {_address}");
	}

	void OnPeerDisconnected(long id) {
		Print($"Peer disconnected ID: {id}");
	}

	void OnServerDisconnected() {
		Print($"Disconnected from server: {_address}");
	}

	[Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	void Print(string text) {
		GD.Print(text);
		_printLabel.Text += text + "\n";
	}

    public override void _Input(InputEvent @event) {
        if (@event is InputEventKey inputEventKey && inputEventKey.Keycode == Key.Enter && inputEventKey.Pressed) {
			if (_connected) {
				var textToPrint = $"{Multiplayer.GetUniqueId()}: {_textEdit.Text}";
				
				Rpc(MethodName.Print, textToPrint);	

				_textEdit.CallDeferred(TextEdit.MethodName.Clear);
			}
			else if (_entering_ip) {
				_address = _ipTextEdit.Text;
				_ipTextEdit.Editable = false;
				ConnectClient();
			}
		}
    }

	public override void _Notification(int what) {
		base._Notification(what);
		
		if (what == NotificationWMCloseRequest) {
			CloseUpnp();
		}
	}
}
