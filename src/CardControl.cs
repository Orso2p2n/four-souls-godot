using Godot;
using System;
using System.Threading.Tasks;

public partial class CardControl : Control
{
	[Export] TextureRect textureRect;
	[Export] CardBase cardBase;

	public override void _Ready() {
		UpdateSprite(true);
	}

	public async Task UpdateSprite(bool setTexture = false) {
		var texture = await cardBase.cardVisual.UpdateAndGetTexture();

		if (setTexture) {
			textureRect.Texture = texture;
		}
	}
}
