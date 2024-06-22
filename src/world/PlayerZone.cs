using Godot;
using System;

[Tool]
public partial class PlayerZone : Node3D
{
    [Export] private CollisionShape3D _collisionShape3D;
    private BoxShape3D _boxShape3D;
        
    [ExportGroup("Area Props")]
    [Export] public Vector2I AreaSize {
        get => _areaSize;
        set {
            SetAreaSize(value);
        }
    }
    private Vector2I _areaSize = Vector2I.One;

    public override void _EnterTree() {
        _boxShape3D = new BoxShape3D();
        _collisionShape3D.Shape = _boxShape3D;
        SetAreaSize(_areaSize);
    }

    public void SetAreaSize(Vector2I areaSize) {
        _areaSize = areaSize;
        
        if (_boxShape3D == null) {
            _boxShape3D = _collisionShape3D.Shape.Duplicate() as BoxShape3D;
            _collisionShape3D.Shape = _boxShape3D;
        }

        var newSize = _boxShape3D.Size;
        newSize.X = areaSize.X;
        newSize.Y = 0;
        newSize.Z = areaSize.Y;
        _boxShape3D.Size = newSize;
    }
}
