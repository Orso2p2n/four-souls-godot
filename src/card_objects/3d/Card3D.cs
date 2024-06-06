using Godot;
using System;
using System.Threading.Tasks;

public partial class Card3D : Node3D
{
	[Export] private Card3DSprite _sprite;
	[Export] private Card3DVisualElement _shadow;

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

	public bool VisualLag { get; private set; }

    public override void _EnterTree() {
		_sprite.Card3D = this;
		_shadow.Card3D = this;
    }

    public override void _Ready() {
		_sprite.Position = Position;
		_shadow.Position = Position;
    }

    public void Init(CardBase cardBase) {
		CardBase = cardBase;
		
		Visible = false;
		cardBase.CardVisual.TextureRefreshed += RefreshSpriteTexture;

		_sprite.SetBackTexture(cardBase.CardResource.BackTextureCropped);
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
		if (VisualLag) {
			var lerpSpeed = 0.3f;
			_sprite.LerpPosition(this, lerpSpeed);
			_shadow.LerpPosition(this, lerpSpeed);
		}
    }

    public void RefreshSpriteTexture(ViewportTexture texture) {
		_sprite.SetFrontTexture(texture);
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
		var targetRot = _sprite.BaseRotation with { Z = targetRotZ };

		if (instant) {
			_sprite.BaseRotation = targetRot;
			_sprite.SetFrontDarkened(down);
			return;
		}

		var tween = _sprite.CreateTween();
		tween.TweenProperty(_sprite, "BaseRotation", targetRot, 0.25f).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.Out);

		await ToSignal(tween, Tween.SignalName.Finished);

		_sprite.SetFrontDarkened(down);
	}

	public void ToggleVisualLag(bool enabled) {
		VisualLag = enabled;
		_sprite.TopLevel = enabled;
	}

	void OnClicked() {
		// _dragged = true;

		// _sprite.TargetHeight = 0.5f;
		// _sprite.TargetOffset2D = Vector2.Up * 0.25f;
	}

	void OnReleased() {
		// _dragged = false;

		// _sprite.TargetHeight = 0f;
		// _sprite.TargetOffset2D = Vector2.Zero;
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
