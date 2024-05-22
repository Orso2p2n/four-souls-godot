using Godot;
using System;

public partial class CardFactory : Node
{
    public static CardFactory ME { get; private set; }

    public override void _EnterTree() {
        base._EnterTree();
    }

    public CardBase CreateCard(CardResource cardResource) {
        CardBase card = (CardBase) Assets.ME.CardScene.Instantiate();

        Script script = cardResource.Script;
        CardBase newCard = card.SafelySetScript<CardBase>(script);

        // Copy exports
        newCard.CardVisual = card.CardVisual;
        newCard.Card3d = card.Card3d;
        newCard.CardControl = card.CardControl;

        newCard.CardResource = cardResource;

        newCard.ChangeParent(this);

        newCard.Init();

        return newCard;
    }

    public CardBase CreateCard3D(CardResource cardResource, Vector3 position) {
        var card = CreateCard(cardResource);
        card.TurnInto3D(position);

        return card;
    }
}
