using Godot;
using Godot.Collections;
using System;

public partial class Lobby : Control
{
	[Export] private VBoxContainer _namesContainer;
	[Export] private PackedScene _nameLabelScene;
	[Export] private Button _startButton;
	[Export] private Button _quitButton;

	Array<LobbyNameLabel> labels = new();

    public override void _EnterTree() {
        NetworkManager.ME.UserAdded += OnUserAdded;
        NetworkManager.ME.UserRemoved += OnUserRemoved;
        NetworkManager.ME.Multiplayer.ServerDisconnected += OnServerDisconnected;
    }

    public override void _Ready() {
		_startButton.Disabled = NetworkManager.ME.State == NetworkState.Client;

        foreach (var user in NetworkManager.ME.Users) {
			AddUserToList(user);
		}
    }

    public override void _ExitTree() {
		NetworkManager.ME.UserAdded -= OnUserAdded;
        NetworkManager.ME.UserRemoved -= OnUserRemoved;
    }

	private void OnUserAdded(NetworkUser user) {
		AddUserToList(user);
	}

	private void OnUserRemoved(NetworkUser user) {
		RemoveUserFromList(user);
	}

	private void OnServerDisconnected() {
		SceneManager.ME.GotoMainMenu();
	}

	private void AddUserToList(NetworkUser user) {
		var label = _nameLabelScene.Instantiate() as LobbyNameLabel;

		label.LinkToUser(user);

		labels.Add(label);

		_namesContainer.AddChild(label);
	}

	private void RemoveUserFromList(NetworkUser user) {
		foreach (var label in labels) {
			if (label.LinkedUser == user) {
				label.Unlink();
				label.QueueFree();
			}
		}
	}

	private void OnQuitButtonPressed() {
		NetworkManager.ME.Disconnect();
	}
}
