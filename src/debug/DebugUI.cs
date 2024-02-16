using Godot;
using System;

public partial class DebugUI : Control
{
	[Export] Button toggleButton;
	[Export] Control menu;

	bool menuShown = false;

	public override void _Ready() {
		CloseMenu();
	}

	public void OpenMenu() {
		menu.Visible = true;
		toggleButton.Visible = false;
	}

	public void CloseMenu() {
		menu.Visible = false;
		toggleButton.Visible = true;
	}

	void _on_toggle_button_pressed() {
		OpenMenu();
	}

	void _on_close_button_pressed() {
		CloseMenu();
	}
}
