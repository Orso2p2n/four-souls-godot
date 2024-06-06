using Godot;
using System;

public partial class Camera : Camera3D
{
	[Export] private float _movementSpeed;
	[Export] private float _sensitivity;

	private Vector2 _mouse_position = new();
	private float _total_pitch = 0f;

	private bool _freeCam;

    public override void _PhysicsProcess(double delta) {
        base._PhysicsProcess(delta);

		if (_freeCam) {
			FreeCam(delta);
		}
    }

    public override void _Input(InputEvent @event) {
        base._Input(@event);

		if (@event is InputEventMouseButton eventMouseButton) {
			if (eventMouseButton.ButtonIndex == MouseButton.Right) {
				if (eventMouseButton.Pressed) {
					if (!_freeCam) {
						_freeCam = true;
						Input.MouseMode = Input.MouseModeEnum.Captured;
					}
				}
				else {
					if (_freeCam) {
						_freeCam = false;
						Input.MouseMode = Input.MouseModeEnum.Visible;
					}
				}
			}
		}

		if (@event is InputEventMouseMotion eventMouseMotion) {
			_mouse_position = eventMouseMotion.Relative;
		}
    }

	private void FreeCam(double delta) {
		var rightVector = this.GetRightVector().Normalized();

		// Move around
		var direction = Vector3.Zero;
		if (Input.IsActionPressed("camera_right")) direction.X += 1;
		if (Input.IsActionPressed("camera_left")) direction.X -= 1;

		if (Input.IsActionPressed("camera_forward")) direction.Z -= 1;
		if (Input.IsActionPressed("camera_backward")) direction.Z += 1;
		
		if (Input.IsActionPressed("camera_up")) direction.Y += 1;
		if (Input.IsActionPressed("camera_down")) direction.Y -= 1;

		if (direction != Vector3.Zero) {
			direction = direction.Normalized();
			direction = direction.Rotated(Vector3.Up, Rotation.Y);
			direction *= _movementSpeed * (float) delta;

			GlobalTranslate(direction);
		}

		// Look around
		var yaw = _mouse_position.X * _sensitivity * (float) delta;
		var pitch = _mouse_position.Y * _sensitivity * (float) delta;

		_mouse_position = Vector2.Zero;

		Rotate(rightVector, Mathf.DegToRad(-pitch));
		
		Vector3 rotationDegrees = RotationDegrees;
		rotationDegrees.X = Mathf.Clamp(rotationDegrees.X, -90, 90);
		RotationDegrees = rotationDegrees;
		
		GlobalRotate(Vector3.Up, Mathf.DegToRad(-yaw));
	}
}
