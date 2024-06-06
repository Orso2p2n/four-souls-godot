using Godot;
using System;

public partial class OtherPlayer : Player
{
    [Export] private OtherHand _hand;

    protected override void OnCardAddedToHand(CardBase card) {
        base.OnCardAddedToHand(card);

        // card.Card3d.FlipDown(true);

        _hand.AddCard(card);
    }

    protected override void OnCardRemovedFromHand(CardBase card) {
        base.OnCardRemovedFromHand(card);

        _hand.RemoveCard(card);
    }
}
