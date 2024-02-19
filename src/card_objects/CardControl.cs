using Godot;
using System;
using System.Threading.Tasks;

public partial class CardControl : Control
{
	[Export] TextureRect textureRect;
	public CardBase cardBase;

	public void Init(CardBase cardBase) {
		this.cardBase = cardBase;

		Visible = false;
		cardBase.cardVisual.TextureRefreshed += UpdateSprite;
	}

    public void UpdateSprite(ViewportTexture texture) {
		textureRect.Texture = texture;
	}
}
