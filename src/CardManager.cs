using Godot;
using System;

public partial class CardManager : Node
{
    [Export] PackedScene cardScene;
    [Export] Node cardContainer;

    public static CardManager ME;

    public override void _Ready() {
        ME = this;
    }

    public CardBase CreateCard(CardResource cardResource) {
        CardBase card = (CardBase) cardScene.Instantiate();

        Script script = cardResource.Script;
        CardBase newCard = card.SafelySetScript<CardBase>(script);

        // Copy exports
        newCard.cardVisual = card.cardVisual;
        newCard.card3d = card.card3d;
        newCard.cardControl = card.cardControl;

        newCard.cardResource = cardResource;

        newCard.ChangeParent(cardContainer);

        newCard.Init();

        return newCard;
    }

    public CardBase CreateCard3D(CardResource cardResource, Vector3 position) {
        var card = CreateCard(cardResource);
        card.TurnInto3D(position);

        return card;
    }
}
