using Godot;
using System;

public partial class OtherPlayer : Player
{
    [Export] private MultiCardsZone _handZone;

    protected override void OnCardAddedToHand(CardBase card) {
        base.OnCardAddedToHand(card);

        card.Card3d.FlipDown(true);

        _handZone.AddCard(card);
    }
}
