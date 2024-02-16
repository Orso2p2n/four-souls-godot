using Godot;
using System;
using System.Threading.Tasks;

public partial class CardVisual : SubViewport
{
	[Signal] public delegate void TextureRefreshedEventHandler(ViewportTexture newTexture);

	[ExportGroup("TextureRects")]
	[Export] public TextureRect maskTextureRect;
	[Export] public TextureRect bgArtTextureRect;
	[Export] public TextureRect borderTextureRect;
	[Export] public TextureRect bottomTextureRect;
	[Export] public TextureRect topTextureRect;
	[Export] public TextureRect statblockTextureRect;
	[Export] public TextureRect rewardTextureRect;
	[Export] public TextureRect soulTextureRect;
	[Export] public TextureRect charmedTextureRect;
	[Export] public TextureRect fgArtTextureRect;

	public CardBase card;

	public void Init() {
		var cardResource = card.cardResource;

		// Set textures
		if (cardResource.BgArt != null) {
			bgArtTextureRect.Texture = cardResource.BgArt;
		}

		if (cardResource.FgArt != null) {
			fgArtTextureRect.Texture = cardResource.FgArt;
		}

		if (cardResource.topTextBoxTexture != null) {
			topTextureRect.Texture = cardResource.topTextBoxTexture;
		}

		if (cardResource.botTextBoxTexture != null) {
			bottomTextureRect.Texture = cardResource.botTextBoxTexture;
		}

		if (cardResource.botTextBoxTexture != null) {
			bottomTextureRect.Texture = cardResource.botTextBoxTexture;
		}

		// Addons
		if (cardResource.soulCount > 0) {
			Texture2D texture = null;
			switch (cardResource.soulCount) {
				case 1:
					texture = StaticTextures.cardStructureAddon1Soul;
					break;

				case 2:
					texture = StaticTextures.cardStructureAddon2Soul;
					break;
			}

			soulTextureRect.Texture = texture;
		}

		if (cardResource.charmed) {
			charmedTextureRect.Texture = StaticTextures.cardStructureAddonCharmed;
		}

		_ = Refresh();
	}

	public async Task Refresh() {
		await UpdateTexture();

		var newTexture = GetTexture();

		EmitSignal(SignalName.TextureRefreshed, newTexture);
	}

	private async Task UpdateTexture() {
		RenderTargetUpdateMode = UpdateMode.Once;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}
}
