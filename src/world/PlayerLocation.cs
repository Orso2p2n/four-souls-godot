using Godot;
using System;

[Tool]
public partial class PlayerLocation : Marker3D
{
    [Export] private Label3D _label;
    [Export] public PlayerZone PlayerZone;

    [ExportGroup("Player Zone Props")]
    [Export] public Vector2I PlayerZoneSize {
        get => _playerZoneSize;
        set {
            _playerZoneSize = value;
            PlayerZone.AreaSize = value;
        }
    }
    private Vector2I _playerZoneSize;
    [Export] public float PlayerZoneOffset {
        get => _playerZoneOffset;
        set {
            SetPlayerZoneOffset(value);
        }
    }
    private float _playerZoneOffset;

    public void SetNumber(int number) {
        _label.Text = number.ToString();
    }

    public void SetPlayerZoneOffset(float zoneOffset) {
        _playerZoneOffset = zoneOffset;

        var newPosition = PlayerZone.Position;
        newPosition.Z = -zoneOffset;
        PlayerZone.Position = newPosition;
    }
}
