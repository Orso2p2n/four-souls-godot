using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class Player : Node
{
    [Export] public Node3D Origin { get; set; }

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
    public CharacterCard CharacterCard { get; set; }

    public int Gold { get; set; }

    public Array<CardBase> CardsInHand { get; set; } = new Array<CardBase>();

    public int PlayerNumber { get; set; }

    public PlayerLocation PlayerLocation { get; set; }

    public int LootPlays { get; set; } = 1;

    public Player PreviousPlayer { get; private set; }
    public Player NextPlayer { get; private set; }

    public virtual void Init(int playerNumber, PlayerLocation playerLocation) {
        PlayerNumber = playerNumber;
        PlayerLocation = playerLocation;

        Origin.GlobalPosition = playerLocation.GlobalPosition;
        Origin.GlobalRotation = playerLocation.GlobalRotation;

        CallDeferred(MethodName.InitDeffered);
    }

    private void InitDeffered() {
        // Set previous player
        var previousPlayerId = PlayerNumber - 1;
        if (previousPlayerId < 0) {
            previousPlayerId = Game.Players.Count - 1;
        }

        PreviousPlayer = Game.Players[previousPlayerId];

        // Set next player
        var nextPlayerId = PlayerNumber + 1;
        if (nextPlayerId >= Game.Players.Count) {
            nextPlayerId = 0;
        }

        NextPlayer = Game.Players[nextPlayerId];
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
    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void StartActionPhase() {
        Rpc(MethodName.IncreaseLootPlays, 1);
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void EndActionPhase() {
        EmitSignal(SignalName.EndActionPhaseRequested);
    }

    // --- LootPlays ---
    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void IncreaseLootPlays(int count) {
        LootPlays += count;
    }

    [Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public virtual void SetLootPlays(int count) {
        LootPlays = count;
    }

    // --- Hand ---
    public void TryAddCardInHand(CardBase card, bool rpc = false) {
        if (card.CanBeInHand) {
            AddCardInHand(card, rpc);
        }
        else {
            Console.Log("Could not add card " + card.CardName + " in player " + PlayerNumber + "'s hand");
        }
    }

    private void AddCardInHand(CardBase card, bool rpc = false) {
        if (rpc) {
            Rpc(MethodName.AddCardInHandID, card.ID);
        }
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

    // --- Zone ---
    public void AddCardInZone(CardBase card) {
        PlayerLocation.PlayerZone.AddCard(card);
    }

    // --- Character ---
    public void SetCharacter(CharacterCard card) {
        CharacterCard = card;
        AddCardInZone(card);
    }

    // Common effects
    [Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void GainOrLoseGold(int amount) {
        Gold += amount;

        Console.Log($"Player {PlayerNumber} has {Gold} gold.");
    }
}
