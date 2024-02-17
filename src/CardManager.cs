using Godot;
using System;

public partial class CardManager : Node
{
    [Export] PackedScene card3DScene;
    [Export] Node cardContainer;

    public static CardManager ME;

    public override void _Ready() {
        ME = this;
    }

    public void CreateCard3D(CardResource cardResource, Vector3 position) {
        Card3D card3D = (Card3D) card3DScene.Instantiate();

        card3D.cardBase = HandleCardBaseSetup(cardResource, card3D.cardBase, card3D);

        card3D.ChangeParent(cardContainer);
        card3D.Position = position;

        card3D.cardBase.Init();
    }

    CardBase HandleCardBaseSetup(CardResource cardResource, CardBase card, Node parent) {
        Script script = cardResource.Script;
        CardBase newCard = card.SafelySetScript<CardBase>(script);
        newCard.cardVisual = card.cardVisual;

        newCard.cardResource = cardResource;

        newCard.ChangeParent(parent);

        return newCard;
    }
}
