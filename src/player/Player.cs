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

    // Getters
    private static Game Game { get { return Game.ME; } }

    // Priority
    public bool HasPriority;
    public PriorityIntention PriorityIntention { get; set; }
    private bool IsActivePlayer { get { return Game.TurnManager.ActivePlayer == this; } }
    private bool IsInActionPhase { get { return IsActivePlayer && Game.TurnManager.CurPhase == TurnPhase.ActionPhase; } }
    
    // Variables
    public CardBase CharacterCard { get; set; }

    public int Gold { get; set; }

    public Array<CardBase> CardsInHand { get; set; } = new Array<CardBase>();

    public int PlayerNumber { get; set; }

    public PlayerLocation PlayerLocation { get; set; }

    public int LootPlays { get; set; }

    public virtual void Init(int playerNumber, PlayerLocation playerLocation) {
        PlayerNumber = playerNumber;
        PlayerLocation = playerLocation;

        _origin.GlobalPosition = playerLocation.GlobalPosition;
        _origin.GlobalRotation = playerLocation.GlobalRotation;
    }

    // Priority
    public virtual void AskForPriorityIntention() {
        PriorityIntention = PriorityIntention.Deciding;
        Console.Log($"Asking player {PlayerNumber} for their priority intention.");
    }

    protected virtual void PriorityIntentionYes() {
        Console.Log($"Player {PlayerNumber} priority intention yes.");
        PriorityIntention = PriorityIntention.Acting;
        EmitSignal(SignalName.PriorityIntentionChosen);
    }

    protected virtual void PriorityIntentionNo() {
        Console.Log($"Player {PlayerNumber} priority intention no.");
        PriorityIntention = PriorityIntention.NotActing;
        EmitSignal(SignalName.PriorityIntentionChosen);
    }

    public async Task<bool> GetPriority() {
        Console.Log($"Priority gotten by player {PlayerNumber}");

        await Task.CompletedTask;

        var rng = new RandomNumberGenerator();
        var decision = rng.RandiRange(0, 1);

        if (decision == 0) {
            Console.Log($"Player {PlayerNumber} hasn't acted during their priority.");
            return false;
        }
        else {
            Console.Log($"Player {PlayerNumber} has acted during their priority.");
            _ = StackManager.ME.StartPriority(PlayerNumber);
            return true;
        }
    }

    public virtual void StartActionPhase() {
        LootPlays += 1;
    }

    public virtual void EndActionPhase() {
        EmitSignal(SignalName.EndActionPhaseRequested);
    }


    // Hand
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

        PrintCardsInHand();

        OnCardAddedToHand(card);
    }

    protected virtual void OnCardAddedToHand(CardBase card) {}

    void PrintCardsInHand() {
        Console.Log("Cards in hand of player " + PlayerNumber + ":");
        foreach (var card in CardsInHand) {
            Console.Log(" - " + card.CardName);
        }
    }


    // Common effects
    public void GainOrLoseGold(int amount) {
        Gold += amount;

        Console.Log($"Player {PlayerNumber} has {Gold} gold.");
    }
}
