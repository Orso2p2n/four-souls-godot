using Godot;
using System;

public partial class FpsCounter : Control
{
	static FpsCounter ME;

	[Export] private Label _label;

    public override void _EnterTree() {
        ME = this;
		Hide();
    }

    public override void _Process(double delta) {
        _label.Text = $"{Engine.GetFramesPerSecond()} FPS";
    }

	public static void Toggle() {
		ME.Visible = !ME.Visible;
	}
}
