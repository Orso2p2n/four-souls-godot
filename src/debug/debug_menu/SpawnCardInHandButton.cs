using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnCardInHandButton : SpawnCardButton
{
	public override CardBase SpawnCard(CardResource cardResource) {
		var card = base.SpawnCard(cardResource);

		foreach (var player in Game.ME.Players) {
			player.TryAddCardInHand(card);
		}

		return card;
	}
}
