using Godot;
using System;
using System.Threading.Tasks;

public partial class CardVisual : SubViewport
{
	[Signal] public delegate void TextureRefreshedEventHandler(ViewportTexture newTexture);

	[Export] public CardVisualComposition Composition { get; set; }

	public CardBase Card { get; private set; }

	public void Init(CardBase card) {
		this.Card = card;
		_ = UpdateVisual();
	}

	private async Task UpdateVisual() {
		var cardResource = Card.CardResource;

		// Set textures
		if (cardResource.BgArt != null) {
			Composition.BgArtTextureRect.Texture = cardResource.BgArt;
		}

		if (cardResource.FgArt != null) {
			Composition.FgArtTextureRect.Texture = cardResource.FgArt;
		}

		if (cardResource.topTextBoxTexture != null) {
			Composition.TopTextureRect.Texture = cardResource.topTextBoxTexture;
		}

		if (cardResource.botTextBoxTexture != null) {
			Composition.BottomTextureRect.Texture = cardResource.botTextBoxTexture;
		}

		if (cardResource.botTextBoxTexture != null) {
			Composition.BottomTextureRect.Texture = cardResource.botTextBoxTexture;
		}

		// Addons
		if (cardResource.SoulCount > 0) {
			Texture2D texture = null;
			switch (cardResource.SoulCount) {
				case 1:
					texture = Assets.ME.CardStructureAddon1Soul;
					break;

				case 2:
					texture = Assets.ME.CardStructureAddon2Soul;
					break;
			}

			Composition.SoulTextureRect.Texture = texture;
		}

		if (cardResource.Charmed) {
			Composition.CharmedTextureRect.Texture = Assets.ME.CardStructureAddonCharmed;
		}

		await Composition.TitleLabel.SetText(cardResource.CardName);

		await Composition.DescriptionContainer.SetDescription(Card);

		_ = RefreshViewportTexture();
	}

	private async Task RefreshViewportTexture() {
		await RenderViewportTexture();

		var newTexture = GetTexture();

		Console.Log("Texture refreshed for: " + Card.CardName);

		EmitSignal(SignalName.TextureRefreshed, newTexture);
	}

	private async Task RenderViewportTexture() {
		RenderTargetUpdateMode = UpdateMode.Once;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}
}
