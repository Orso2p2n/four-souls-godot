using Godot;
using Godot.Collections;
using System;

public partial class CardFactory : Node
{
    public static CardFactory ME { get; private set; }

    public Array<CardBase> AllCards = new();

    public override void _EnterTree() {
        base._EnterTree();

        ME = this;
    }

    public CardBase GetCard(int id) {
        return AllCards[id];
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void CreateCardFromPath(string path) {
        var cardResource = ResourceLoader.Load(path) as CardResource;
        _CreateCard(cardResource);
    } 

    public CardBase CreateCard(CardResource cardResource, bool rpc = false) {
        if (rpc) {
            Rpc(MethodName.CreateCardFromPath, cardResource.ResourcePath);
        }
        
        return _CreateCard(cardResource);
    }

    private CardBase _CreateCard(CardResource cardResource) {
        CardBase card = (CardBase) Assets.ME.CardScene.Instantiate();

        Script script = cardResource.Script;
        CardBase newCard = card.SafelySetScript<CardBase>(script);

        // Copy exports
        newCard.CardVisual = card.CardVisual;
        newCard.Card3d = card.Card3d;

        newCard.CardResource = cardResource;

        newCard.ID = AllCards.Count;
        AllCards.Add(newCard);

        newCard.ChangeParent(this);

        newCard.Init();

        return newCard;
    }

    public void DeleteCard(int id) {
        var card = GetCard(id);
        
        if (card == null) {
            return;
        }

        AllCards[id] = null;
        card.QueueFree();
    }
}
