using Godot;
using System;
using System.Threading.Tasks;

public partial class CardVisual : SubViewport
{
	[Signal] public delegate void TextureRefreshedEventHandler(ViewportTexture newTexture);

	[Export] public CardVisualComposition composition;

	public CardBase card;

	public void Init() {
		var cardResource = card.cardResource;

		// Set textures
		if (cardResource.BgArt != null) {
			composition.bgArtTextureRect.Texture = cardResource.BgArt;
		}

		if (cardResource.FgArt != null) {
			composition.fgArtTextureRect.Texture = cardResource.FgArt;
		}

		if (cardResource.topTextBoxTexture != null) {
			composition.topTextureRect.Texture = cardResource.topTextBoxTexture;
		}

		if (cardResource.botTextBoxTexture != null) {
			composition.bottomTextureRect.Texture = cardResource.botTextBoxTexture;
		}

		if (cardResource.botTextBoxTexture != null) {
			composition.bottomTextureRect.Texture = cardResource.botTextBoxTexture;
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

			composition.soulTextureRect.Texture = texture;
		}

		if (cardResource.charmed) {
			composition.charmedTextureRect.Texture = StaticTextures.cardStructureAddonCharmed;
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
