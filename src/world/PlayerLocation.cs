using Godot;
using System;

public partial class PlayerLocation : Marker3D
{
    [Export] private Label3D _label;
    [Export] public PlayerZone PlayerZone;

    [ExportGroup("Player Zone Props")]
    [Export] public Vector2I PlayerZoneSize { get; set; }
    [Export] public float PlayerZoneOffset { get; set; }

    public override void _Ready() {
        PlayerZone.SetAreaSize(PlayerZoneSize);
        SetPlayerZoneOffset();
    }

    public void SetNumber(int number) {
        _label.Text = number.ToString();
    }

    private void SetPlayerZoneOffset() {
        var newPosition = PlayerZone.Position;
        newPosition.Z -= PlayerZoneOffset;
        PlayerZone.Position = newPosition;
    }
}
