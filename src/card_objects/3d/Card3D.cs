using Godot;
using System;
using System.Threading.Tasks;

public partial class Card3D : Node3D
{
	[Export] private Card3DModel _model;

	public CardBase CardBase { get; set; }

	public bool FaceDown;

	public Vector2 Position2D {
		get {
			return Position.GetXZ();
		}
		set {
			Position = Position.SetXZ(value.X, value.Y);
		}
	}

	private bool _dragged;

    public override void _EnterTree() {
		_model.Card3D = this;
    }

    public override void _Ready() {

    }

    public void Init(CardBase cardBase) {
		CardBase = cardBase;
		
		Visible = false;
		cardBase.CardVisual.TextureRefreshed += RefreshSpriteTexture;

		_model.SetBackTexture(cardBase.CardResource.BackTextureCropped);
	}

    public override void _Process(double delta) {
		if (_dragged) {
			var viewport = GetViewport();
			var camera = viewport.GetCamera3D();
			var viewportMousePos = viewport.GetMousePosition();
		
			var cameraHeight = camera.Position.Y;

			var worldPos = camera.ProjectPosition(viewportMousePos, cameraHeight);

			Position2D = worldPos.GetXZ();
		}
    }

    public override void _PhysicsProcess(double delta) {

    }

    public void RefreshSpriteTexture(ViewportTexture texture) {
		_model.SetFrontTexture(texture);
	}

	public void Flip(bool instant = false) {
		SetFace(!FaceDown, instant);
	}

	public void FlipUp(bool instant = false) {
		SetFace(true, instant);
	}

	public void FlipDown(bool instant = false) {
		SetFace(false, instant);
	}

	public async void SetFace(bool down, bool instant = false) {
		FaceDown = down;

		var targetRotZ = down ? -180 : 180;
		var targetRot = _model.BaseRotation with { Z = targetRotZ };

		if (instant) {
			_model.BaseRotation = targetRot;
			_model.SetFrontDarkened(down);
			return;
		}

		var tween = _model.CreateTween();
		tween.TweenProperty(_model, "BaseRotation", targetRot, 0.25f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);

		await ToSignal(tween, Tween.SignalName.Finished);

		_model.SetFrontDarkened(down);
	}

	void OnClicked() {

	}

	void OnReleased() {

	}

	void _on_area_3d_mouse_entered() {}

	void _on_area_3d_mouse_exited() {}

	void _on_area_3d_input_event(Node camera, InputEvent inputEvent, Vector3 position, Vector3 normal, int shape_idx) {
		if (inputEvent is InputEventMouseButton eventMouseButton) {
			if (eventMouseButton.ButtonIndex == MouseButton.Left) {
				if (eventMouseButton.IsPressed()) {
					OnClicked();
				}
				else {
					OnReleased();
				}
			}
		}
	}
}
