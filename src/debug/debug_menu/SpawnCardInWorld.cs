using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnCardInWorld : SpawnCardButton
{
	public override CardBase SpawnCard(CardResource cardResource) {
		var card = base.SpawnCard(cardResource);

		card.Show3D();

		return card;
	}
}
