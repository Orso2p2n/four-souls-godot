using Godot;
using System;

public partial class PlayerLocation : Marker3D
{
    [Export] private Label3D _label;

	public void SetNumber(int number) {
        _label.Text = number.ToString();
    }
}
