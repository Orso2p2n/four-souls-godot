using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class Player : Node
{
    [Export] protected Node3D _origin;

    // Signals
    [Signal] public delegate void EndActionPhaseRequestedEventHandler();
    [Signal] public delegate void PriorityIntentionChosenEventHandler();
    [Signal] public delegate void PriorityActionEventHandler(bool passesPriority);

    // Getters
    private static Game Game { get { return Game.ME; } }

    // Priority
    public bool HasPriority;
    public PriorityIntention PriorityIntention { get; set; }
    public bool IsActivePlayer { get { return Game.TurnManager.ActivePlayer == this; } }
    public bool IsInActionPhase { get { return IsActivePlayer && Game.TurnManager.CurPhase == TurnPhase.ActionPhase; } }
    
    // Variables
    public CardBase CharacterCard { get; set; }

    public int Gold { get; set; }

    public Array<CardBase> CardsInHand { get; set; } = new Array<CardBase>();

    public int PlayerNumber { get; set; }

    public PlayerLocation PlayerLocation { get; set; }

    public int LootPlays { get; set; } = 1;

    public virtual void Init(int playerNumber, PlayerLocation playerLocation) {
        PlayerNumber = playerNumber;
        PlayerLocation = playerLocation;

        _origin.GlobalPosition = playerLocation.GlobalPosition;
        _origin.GlobalRotation = playerLocation.GlobalRotation;
    }

    // --- Priority ---
    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void AskForPriorityIntention() {
        PriorityIntention = PriorityIntention.Deciding;
        Console.Log($"Asking player {PlayerNumber} for their priority intention.");
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected virtual void PriorityIntentionYes() {
        Console.Log($"Player {PlayerNumber} priority intention yes.");
        PriorityIntention = PriorityIntention.Acting;
        EmitSignal(SignalName.PriorityIntentionChosen);
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected virtual void PriorityIntentionNo() {
        Console.Log($"Player {PlayerNumber} priority intention no.");
        PriorityIntention = PriorityIntention.NotActing;
        EmitSignal(SignalName.PriorityIntentionChosen);
    }

    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void GetPriority() {
        Console.Log($"Priority gotten by player {PlayerNumber}");

        HasPriority = true;
    }

    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void RemovePriority() {
        HasPriority = false;
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    protected void EmitPriorityAction(bool passesPriority) {
        EmitSignal(SignalName.PriorityAction, passesPriority);
    }

    // --- Action Phase ---
    public virtual void StartActionPhase() {
        Rpc(MethodName.IncreaseLootPlays, 1);
    }

    public virtual void EndActionPhase() {
        EmitSignal(SignalName.EndActionPhaseRequested);
    }

    // --- LootPlays ---
    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void IncreaseLootPlays(int count) {
        LootPlays += count;
    }

    // --- Hand ---
    public void TryAddCardInHand(CardBase card) {
        if (card.CanBeInHand) {
            AddCardInHand(card);
        }
        else {
            Console.Log("Could not add card " + card.CardName + " in player " + PlayerNumber + "'s hand");
        }
    }

    private void AddCardInHand(CardBase card) {
        Rpc(MethodName.AddCardInHandID, card.ID);
        _AddCardInHand(card);
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void AddCardInHandID(int id) {
        var card = CardFactory.ME.GetCard(id);
        _AddCardInHand(card);
    }

    private void _AddCardInHand(CardBase card) {
        CardsInHand.Add(card);
        card.OnAddedToPlayerHand(this);

        OnCardAddedToHand(card);
    }

    public void RemoveCardFromHand(CardBase card) {
        Rpc(MethodName.RemoveCardFromHandID, card.ID);
        _RemoveCardFromHand(card);
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RemoveCardFromHandID(int id) {
        var card = CardFactory.ME.GetCard(id);
        _RemoveCardFromHand(card);
    }

    private void _RemoveCardFromHand(CardBase card) {
        CardsInHand.Remove(card);

        OnCardRemovedFromHand(card);
    }

    protected virtual void OnCardAddedToHand(CardBase card) {}

    protected virtual void OnCardRemovedFromHand(CardBase card) {}

    void PrintCardsInHand() {
        Console.Log("Cards in hand of player " + PlayerNumber + ":");
        foreach (var card in CardsInHand) {
            Console.Log(" - " + card.CardName);
        }
    }


    // Common effects
    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void GainOrLoseGold(int amount) {
        Gold += amount;

        Console.Log($"Player {PlayerNumber} has {Gold} gold.");
    }
}
