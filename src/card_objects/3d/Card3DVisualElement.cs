using Godot;
using System;

public partial class Card3DVisualElement : Node3D
{
	[Export] private bool _canRotate3d;
	// [Export] private bool _canRotate2d;

	public Card3D Card3D { get; set; }

	public Vector3 BaseRotation { 
		get {
			return _baseRotation;
		}		
		set {
			_baseRotation = value;
			RefreshRotation();
		}
	}

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
		BaseRotation = Vector3.Zero;
    }

    public override void _Process(double delta) {
		RefreshRotation();
    }

	private void RefreshRotation() {
		var rot = BaseRotation + _offsetRotation;

		if (_oldRot != rot) {
			_oldRot = rot;
			RotationDegrees = rot;
		}
	}

	// Only when top level
    virtual public void LerpPosition(Card3D card3D, float lerpSpeed) {
		if (Position != card3D.Position) {
			Position = Position.Lerp(card3D.Position, lerpSpeed);

			RotateTowards(card3D.Position2D);

			if (Position.DistanceTo(card3D.Position) < 0.01f) {
				Position = card3D.Position;
				_offsetRotation = Vector3.Zero;
			}
		}
	}

	virtual protected void RotateTowards(Vector2 targetPosition) {
		if (!TopLevel || !_canRotate3d) {
			return;
		}

		var angleToTarget = Position2D.AngleToPoint(targetPosition);
		var degreeAngle = Mathf.RadToDeg(angleToTarget);

		var xDir = Mathf.Sign(targetPosition.X - Position2D.X);
		var yDir = Mathf.Sign(targetPosition.Y - Position2D.Y);

		var xDistToTarget = Position2D.DistanceTo(new Vector2(targetPosition.X, Position2D.Y));
		var yDistToTarget = Position2D.DistanceTo(new Vector2(Position2D.X, targetPosition.Y));

		var distForMaxAngle = 0.5f;
		var xLerpSpeed = Mathf.Clamp(xDistToTarget / distForMaxAngle, 0f, 1f);
		var yLerpSpeed = Mathf.Clamp(yDistToTarget / distForMaxAngle, 0f, 1f);

		var maxAngleOffset = 35f;

		var xRotation = Mathf.Lerp(0f, maxAngleOffset, xLerpSpeed) * -xDir;
		var yRotation = Mathf.Lerp(0f, maxAngleOffset, yLerpSpeed) * yDir;

		_offsetRotation = new Vector3(yRotation, 0, xRotation);
	}
}
