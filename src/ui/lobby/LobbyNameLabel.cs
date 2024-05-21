using Godot;
using System;

public partial class LobbyNameLabel : Label
{
	public NetworkUser LinkedUser { get; set; }

	public void LinkToUser(NetworkUser user) {
		LinkedUser = user;

		Text = user.Id.ToString();

		if (user.IsHost) {
			Text += " (Host)";
		}

		if (user.IsSelf) {
			Text += " (You)";
		}
	}

	public void Unlink() {
		LinkedUser = null;
	}
}
