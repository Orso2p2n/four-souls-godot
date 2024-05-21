using Godot;
using System;

public partial class MainMenu : Control
{
	[Export] private Button _hostButton;
	[Export] private Button _joinButton;
	
	[Export] private Panel _addressPanel;
	[Export] private LineEdit _addressLineEdit;
	[Export] private Button _connectButton;

    public override void _EnterTree() {
        _addressPanel.Hide();
    }

    void SetButtonsActive(bool active) {
		_hostButton.Disabled = _joinButton.Disabled = !active;
	}

	void OnHostButtonPressed() {
		SetButtonsActive(false);

		NetworkManager.ME.ServerCreated += OnServerCreated;

		NetworkManager.ME.Host();
	}

	void OnServerCreated() {
		SceneManager.ME.GotoLobby();
	}

	void OnJoinButtonPressed() {
		SetButtonsActive(false);

		NetworkManager.ME.Multiplayer.ConnectedToServer += OnConnectedToServer;

		_addressPanel.Show();
	}

	void OnClosePanelButtonPressed() {
		_addressPanel.Hide();

		NetworkManager.ME.Multiplayer.ConnectedToServer -= OnConnectedToServer;

		SetButtonsActive(true);
	}

	void OnConnectButtonPressed() {
		NetworkManager.ME.Address = _addressLineEdit.Text;
		_addressLineEdit.Clear();

		NetworkManager.ME.Connect();
	}

	void OnConnectedToServer() {
		SceneManager.ME.GotoLobby();
	}
}
