using Godot;
using System;
using System.Threading.Tasks;

public partial class CardVisual : SubViewport
{
	[Signal] public delegate void TextureRefreshedEventHandler(ViewportTexture newTexture);

	[Export] public CardVisualComposition Composition { get; set; }

	public CardBase Card { get; private set; }

	private CardVisualStatContainer _visibleStatContainer;

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

		if (cardResource.TopTextBoxTexture != null) {
			Composition.TopTextureRect.Texture = cardResource.TopTextBoxTexture;
		}

		if (cardResource.BotTextBoxTexture != null) {
			Composition.BottomTextureRect.Texture = cardResource.BotTextBoxTexture;
		}

		if (cardResource.BotTextBoxTexture != null) {
			Composition.BottomTextureRect.Texture = cardResource.BotTextBoxTexture;
		}

		// Stats
		if (cardResource.StatblockTexture != null) {
			Composition.StatblockTextureRect.Texture = cardResource.StatblockTexture;
		}

		switch (cardResource.StatType) {
			case StatType.Monster:
				_visibleStatContainer = Composition.MonsterStats;
				break;

			case StatType.Character:
				_visibleStatContainer = Composition.CharacterStats;
				break;
		}

		if (_visibleStatContainer != null) {
			_visibleStatContainer.Show();
			_visibleStatContainer.SetHp(cardResource.HpStat);
			_visibleStatContainer.SetDice(cardResource.DiceStat);
			_visibleStatContainer.SetAtk(cardResource.AtkStat);
		}

		// Addons
		if (cardResource.SoulCount > 0) {
			Texture2D texture = null;
			switch (cardResource.SoulCount) {
				case 1:
					texture = Assets.ME.Addon1Soul;
					break;

				case 2:
					texture = Assets.ME.Addon2Soul;
					break;
			}

			Composition.SoulTextureRect.Texture = texture;
		}

		if (cardResource.Charmed) {
			Composition.CharmedTextureRect.Texture = Assets.ME.AddonCharmed;
		}

		// Text
		await Composition.TitleLabel.SetText(cardResource.CardName);

		await Composition.DescriptionContainer.SetDescription(Card);

		_ = RefreshViewportTexture();
	}

	private async Task RefreshViewportTexture() {
		await RenderViewportTexture();

		var newTexture = GetTexture();

		EmitSignal(SignalName.TextureRefreshed, newTexture);
	}

	private async Task RenderViewportTexture() {
		RenderTargetUpdateMode = UpdateMode.Once;

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
	}
}
