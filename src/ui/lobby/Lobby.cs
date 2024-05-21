using Godot;
using Godot.Collections;
using System;

public partial class Lobby : Control
{
	[Export] private VBoxContainer _namesContainer;
	[Export] private PackedScene _nameLabelScene;

	Array<LobbyNameLabel> labels = new();

    public override void _EnterTree() {
        NetworkManager.ME.UserAdded += OnUserAdded;
        NetworkManager.ME.UserRemoved += OnUserRemoved;
        NetworkManager.ME.Multiplayer.ServerDisconnected += OnServerDisconnected;
    }

    public override void _Ready() {
        foreach (var user in NetworkManager.ME.users) {
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
