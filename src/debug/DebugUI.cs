using Godot;
using System;

public partial class DebugUI : Control
{
	[Export] private Button _toggleButton;
	[Export] private Control _menu;

	private bool _menuShown = false;

	public override void _Ready() {
		CloseMenu();
	}

	public void OpenMenu() {
		_menu.Visible = true;
		_toggleButton.Visible = false;
	}

	public void CloseMenu() {
		_menu.Visible = false;
		_toggleButton.Visible = true;
	}

	void _on_toggle_button_pressed() {
		OpenMenu();
	}

	void _on_close_button_pressed() {
		CloseMenu();
	}

	void _on_menu_gui_input(InputEvent _event) {
		if (_event is InputEventMouseButton eventMouseButton) {
			if (eventMouseButton.ButtonIndex == MouseButton.Left) {
				if (eventMouseButton.IsPressed()) {
					CloseMenu();
				}
			}
		}
	}
}
