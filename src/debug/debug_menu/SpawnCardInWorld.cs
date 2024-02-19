using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnCardInWorld : SpawnCardButton
{
	public override CardBase SpawnCard(CardResource cardResource) {
		var card = base.SpawnCard(cardResource);

		card.TurnInto3D(Vector3.Zero);

		return card;
	}
}
