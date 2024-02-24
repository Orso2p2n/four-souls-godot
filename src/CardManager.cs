using Godot;
using System;

public partial class CardManager : Node
{
    [Export] private PackedScene _cardScene;
    [Export] private Node _cardContainer;

    public static CardManager ME { get; set; }

    public override void _Ready() {
        ME = this;
    }

    public CardBase CreateCard(CardResource cardResource) {
        CardBase card = (CardBase) _cardScene.Instantiate();

        Script script = cardResource.Script;
        CardBase newCard = card.SafelySetScript<CardBase>(script);

        // Copy exports
        newCard.CardVisual = card.CardVisual;
        newCard.Card3d = card.Card3d;
        newCard.CardControl = card.CardControl;

        newCard.CardResource = cardResource;

        newCard.ChangeParent(_cardContainer);

        newCard.Init();

        return newCard;
    }

    public CardBase CreateCard3D(CardResource cardResource, Vector3 position) {
        var card = CreateCard(cardResource);
        card.TurnInto3D(position);

        return card;
    }
}
