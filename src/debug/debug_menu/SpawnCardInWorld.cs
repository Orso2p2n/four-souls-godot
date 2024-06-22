using Godot;
using System;
using System.Collections.Generic;

public partial class SpawnCardInWorld : SpawnCardButton
{
	public override CardBase SpawnCard(CardResource cardResource) {
		var card = base.SpawnCard(cardResource);

		Rpc(MethodName.AddToZone, MainPlayer.ME.PlayerNumber, card.ID);	

		return card;
	}

	[Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void AddToZone(int playerNumber, int cardId) {
		var card = CardFactory.ME.GetCard(cardId);
		var player = Game.ME.Players[playerNumber];
		player.AddCardInZone(card);
	}
}
