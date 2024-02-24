using Godot;
using System;
using System.Threading.Tasks;

public partial class Card2D : Node2D
{
	[Export] private Sprite2D _sprite2D;
	private CardBase _cardBase;

	public override void _Ready() {
		_cardBase.CardVisual.TextureRefreshed += UpdateSprite;
	}

    public void UpdateSprite(ViewportTexture texture) {
		_sprite2D.Texture = texture;
	}
}
