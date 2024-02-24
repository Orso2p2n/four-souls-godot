using Godot;
using System;
using System.Threading.Tasks;

public partial class Card3D : Node3D
{
	[Export] private Sprite3D _sprite3D;

	public CardBase CardBase { get; set; }

	private bool _dragged;

	public void Init(CardBase cardBase) {
		CardBase = cardBase;
		
		Visible = false;
		cardBase.CardVisual.TextureRefreshed += UpdateSprite;
	}

    public override void _Process(double delta) {
		if (_dragged) {
			var viewport = GetViewport();
			var camera = viewport.GetCamera3D();
			var viewportMousePos = viewport.GetMousePosition();
		
			var cameraHeight = camera.Position.Y;

			var worldPos = camera.ProjectPosition(viewportMousePos, cameraHeight);
			worldPos.Y = 0;

			Position = worldPos;
		}
    }

    public void UpdateSprite(ViewportTexture texture) {
		_sprite3D.Texture = texture;
	}

	void OnClicked() {
		GD.Print("On Card3D clicked");

		_dragged = true;
	}

	void OnReleased() {
		GD.Print("On Card3D released");

		_dragged = false;
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
