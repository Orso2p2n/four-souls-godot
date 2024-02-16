using Godot;
using System;
using System.Threading.Tasks;

public partial class Card3D : Node3D
{
	[Export] Sprite3D sprite3D;
	[Export] CardBase cardBase;

	bool dragged;

	public override void _Ready() {
		UpdateSprite(true);

		GD.Print("Card3D ready");
	}

    public override void _Process(double delta) {
		if (dragged) {
			var viewport = GetViewport();
			var camera = viewport.GetCamera3D();
			var viewportMousePos = viewport.GetMousePosition();
		
			var cameraHeight = camera.Position.Y;

			var worldPos = camera.ProjectPosition(viewportMousePos, cameraHeight);
			worldPos.Y = 0;

			Position = worldPos;
		}
    }

    public async Task UpdateSprite(bool setTexture = false) {
		var texture = await cardBase.cardVisual.UpdateAndGetTexture();

		if (setTexture) {
			sprite3D.Texture = texture;
		}
	}

	void OnClicked() {
		GD.Print("On Card3D clicked");

		dragged = true;
	}

	void OnReleased() {
		GD.Print("On Card3D released");

		dragged = false;
	}

	void _on_area_3d_mouse_entered() {}

	void _on_area_3d_mouse_exited() {}

	void _on_area_3d_input_event(Node camera, InputEvent _event, Vector3 position, Vector3 normal, int shape_idx) {
		if (_event is InputEventMouseButton eventMouseButton) {
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
