using Godot;
using System;
using System.Threading.Tasks;

public partial class Card2D : Node2D
{
	[Export] Sprite2D sprite2D;
	[Export] CardBase cardBase;

	public override void _Ready() {
		UpdateSprite(true);
	}

	public async Task UpdateSprite(bool setTexture = false) {
		var texture = await cardBase.cardVisual.UpdateAndGetTexture();

		if (setTexture) {
			sprite2D.Texture = texture;
		}
	}
}
