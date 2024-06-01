using Godot;
using Godot.Collections;
using System;

public partial class Deck : Node
{
	public Array<CardResource> Cards { get; set; }

	public void Init(Array<CardResource> cards) {
		Cards = cards;

		PrintCards();
	}

	private void PrintCards() {
		foreach (var card in Cards) {
			Console.Log(card.CardName);
		}
	}
}
