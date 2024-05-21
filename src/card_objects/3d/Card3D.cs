using Godot;
using System;
using System.Threading.Tasks;

public partial class Card3D : Node3D
{
	[Export] private Card3DSprite _sprite3D;
	[Export] private Card3DVisualElement _shadow;

	public CardBase CardBase { get; set; }

	public Vector2 Position2D {
		get {
			return Position.GetXZ();
		}
		set {
			Position = Position.SetXZ(value.X, value.Y);
		}
	}

	private bool _dragged;

	public void Init(CardBase cardBase) {
		CardBase = cardBase;
		
		Visible = false;
		cardBase.CardVisual.TextureRefreshed += RefreshSpriteTexture;
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
		var lerpSpeed = 0.3f;
		_sprite3D.LerpPosition(this, lerpSpeed);
		_shadow.LerpPosition(this, lerpSpeed);
    }

    public void RefreshSpriteTexture(ViewportTexture texture) {
		_sprite3D.Texture = texture;
	}

	void OnClicked() {
		Console.Log("On Card3D clicked");

		_dragged = true;

		_sprite3D.TargetHeight = 0.5f;
		_sprite3D.TargetOffset2D = Vector2.Up * 0.25f;
	}

	void OnReleased() {
		Console.Log("On Card3D released");

		_dragged = false;

		_sprite3D.TargetHeight = 0f;
		_sprite3D.TargetOffset2D = Vector2.Zero;
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
