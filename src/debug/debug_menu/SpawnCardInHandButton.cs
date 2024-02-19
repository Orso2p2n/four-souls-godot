using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnCardInHandButton : SpawnCardButton
{
	public override CardBase SpawnCard(CardResource cardResource) {
		var card = base.SpawnCard(cardResource);

		MainPlayer.ME.AddCardInHand(card);

		return card;
	}
}
