using Godot;
using Godot.Collections;
using System;

public partial class Deck : Node
{
	[Export] private Deck3D _deck3d;

	public DeckTypeResource DeckType;

	public Array<CardResource> Cards { get; set; }

	public void Init(Array<CardResource> cards, DeckTypeResource deckType, DeckLocation deckLocation = null) {
		Cards = cards;
		DeckType = deckType;

		_deck3d.Init(this);

		if (deckLocation != null) {
			_deck3d.GlobalPosition = deckLocation.GetDeckLocation();
		}
		else {
			_deck3d.GlobalPosition = Vector3.Zero;
			_deck3d.Visible = false;
		}

		Shuffle();

		// PrintCards();

		RefreshModelCardsCount();
	}

	private void PrintCards() {
		foreach (var card in Cards) {
			Console.Log(card.CardName);
		}
	}

	private void Shuffle() {
		Cards.Shuffle(Game.ME.Rng);
	}

	private void RefreshModelCardsCount() {
		_deck3d.SetCardsCount(Cards.Count);
	}

	public Array<CardBase> CreateTopCards(int count) {
		var cards = new Array<CardBase>();
		for (int i = 0; i < count; i++) {
			var card = CreateTopCard(refresh: false);

			if (card == null) {
				break;
			}

			cards.Add(card);
		}

		RefreshModelCardsCount();

		return cards;
	}

	public CardBase CreateTopCard(bool refresh = true) {
		if (Cards.Count == 0) {
			return null;
		}

		var cardResource = Cards[0];
		Cards.RemoveAt(0);
		var card = Game.ME.CardFactory.CreateCard(cardResource);

		var height = _deck3d.Mesh.GetAabb().Size.Y + 0.1f;
		var cardPos = _deck3d.GlobalPosition + Vector3.Up * height;
		card.Card3d.Position = cardPos;
		card.Card3d.FlipDown(true);

		if (refresh) {
			RefreshModelCardsCount();
		}

		return card;
	}
}
