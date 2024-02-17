using Godot;
using System;
using System.Threading.Tasks;

public partial class Card2D : Node2D
{
	[Export] Sprite2D sprite2D;
	[Export] CardBase cardBase;

	public override void _Ready() {
		cardBase.cardVisual.TextureRefreshed += UpdateSprite;
	}

    public void UpdateSprite(ViewportTexture texture) {
		sprite2D.Texture = texture;
	}
}
