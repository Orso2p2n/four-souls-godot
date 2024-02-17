using Godot;
using System;
using System.Threading.Tasks;

public partial class CardControl : Control
{
	[Export] TextureRect textureRect;
	[Export] CardBase cardBase;

	public override void _Ready() {
		cardBase.cardVisual.TextureRefreshed += UpdateSprite;
	}

    public void UpdateSprite(ViewportTexture texture) {
		textureRect.Texture = texture;
	}
}
