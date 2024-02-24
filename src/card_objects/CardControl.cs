using Godot;
using System;
using System.Threading.Tasks;

public partial class CardControl : Control
{
	[Export] private TextureRect _textureRect;

	public CardBase CardBase { get; set; }

	public void Init(CardBase cardBase) {
		CardBase = cardBase;

		Visible = false;
		cardBase.CardVisual.TextureRefreshed += UpdateSprite;
	}

    public void UpdateSprite(ViewportTexture texture) {
		_textureRect.Texture = texture;
	}
}
