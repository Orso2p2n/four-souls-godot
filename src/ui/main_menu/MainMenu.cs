using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] private Button _hostButton;
	[Export] private Button _hostLocalButton;
	[Export] private Button _joinButton;
	
	[Export] private Panel _addressPanel;
	[Export] private LineEdit _addressLineEdit;
	[Export] private Button _connectButton;

    public override void _EnterTree() {
        _addressPanel.Hide();
    }

    public override void _Ready() {
		NetworkManager.ME.ServerCreated += OnServerCreated;
		NetworkManager.ME.ServerCreationFailed += OnServerCreationFailed;
		NetworkManager.ME.ConnectedToLobby += OnConnectedToLobby;

		NetworkManager.ME.GameState = GameState.InMenu;
    }

    void SetButtonsActive(bool active) {
		_hostButton.Disabled = _hostLocalButton.Disabled = _joinButton.Disabled = !active;
	}

	void OnHostButtonPressed() {
		SetButtonsActive(false);

		NetworkManager.ME.HostUpnp();
	}

	void OnHostLocalButtonPressed() {
		SetButtonsActive(false);

		NetworkManager.ME.HostLocal();
	}

	void OnServerCreated() {
		SceneManager.ME.GotoLobby();
	}

	void OnServerCreationFailed() {
		SetButtonsActive(true);
	}

	void OnJoinButtonPressed() {
		SetButtonsActive(false);

		_addressPanel.Show();
	}

	void OnClosePanelButtonPressed() {
		_addressPanel.Hide();

		SetButtonsActive(true);
	}

	void OnConnectButtonPressed() {
		NetworkManager.ME.Address = _addressLineEdit.Text;
		_addressLineEdit.Clear();

		NetworkManager.ME.Connect();
	}

	void OnConnectedToLobby() {
		SceneManager.ME.GotoLobby();
	}
}
