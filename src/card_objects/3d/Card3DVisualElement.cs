using Godot;
using System;

public partial class Card3DVisualElement : Sprite3D
{
	[Export] private bool _canRotate3d;
	// [Export] private bool _canRotate2d;

	private Vector3 _baseRotation;
	private Vector3 _offsetRotation;
	private Vector3 _oldRot;

	virtual public Vector2 Position2D {
		get {
			return Position.GetXZ();
		}
		set {
			Position = Position.SetXZ(value.X, value.Y);
		}
	}

    public override void _Ready() {
		_baseRotation = RotationDegrees;
    }

    public override void _Process(double delta) {
		var rot = _baseRotation + _offsetRotation;

		if (_oldRot != rot) {
			_oldRot = rot;
			RotationDegrees = rot;
			// GD.Print("Base rotation: " + _baseRotation + ", Offset Rotation: " + _offsetRotation + ", Rotation: " + rot);
		}


    }

    virtual public void LerpPosition(Card3D card3D, float lerpSpeed) {
		if (Position2D != card3D.Position2D) {
			Position2D = Position2D.Lerp(card3D.Position2D, lerpSpeed);

			RotateTowards(card3D.Position2D);

			if (Position2D.DistanceTo(card3D.Position2D) < 0.01f) {
				Position2D = card3D.Position2D;
			}
		}
	}

	virtual protected void RotateTowards(Vector2 targetPosition) {
		var angleToTarget = Position2D.AngleToPoint(targetPosition);
		var degreeAngle = Mathf.RadToDeg(angleToTarget);

		var xDir = Mathf.Sign(targetPosition.X - Position2D.X);
		var yDir = Mathf.Sign(targetPosition.Y - Position2D.Y);

		var xDistToTarget = Position2D.DistanceTo(new Vector2(targetPosition.X, Position2D.Y));
		var yDistToTarget = Position2D.DistanceTo(new Vector2(Position2D.X, targetPosition.Y));

		var distForMaxAngle = 0.5f;
		var xLerpSpeed = Mathf.Clamp(xDistToTarget / distForMaxAngle, 0f, 1f);
		var yLerpSpeed = Mathf.Clamp(yDistToTarget / distForMaxAngle, 0f, 1f);

		if (_canRotate3d) {
			var maxAngleOffset = 35f;

			var xRotation = Mathf.Lerp(0f, maxAngleOffset, xLerpSpeed) * -xDir;
			var yRotation = Mathf.Lerp(0f, maxAngleOffset, yLerpSpeed) * yDir;

			_offsetRotation = new Vector3(yRotation, 0, xRotation);
		}
	}
}
