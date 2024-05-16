using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class Player : Node
{
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
    }

    // Priority
    public virtual void AskForPriorityIntention() {
        PriorityIntention = PriorityIntention.Deciding;
        GD.Print($"Asking player {PlayerNumber} for their priority intention.");
    }

    protected virtual void PriorityIntentionYes() {
        GD.Print($"Player {PlayerNumber} priority intention yes.");
        PriorityIntention = PriorityIntention.Acting;
        EmitSignal(SignalName.PriorityIntentionChosen);
    }

    protected virtual void PriorityIntentionNo() {
        GD.Print($"Player {PlayerNumber} priority intention no.");
        PriorityIntention = PriorityIntention.NotActing;
        EmitSignal(SignalName.PriorityIntentionChosen);
    }

    public async Task<bool> GetPriority() {
        GD.Print($"Priority gotten by player {PlayerNumber}");

        await Task.CompletedTask;

        var rng = new RandomNumberGenerator();
        var decision = rng.RandiRange(0, 1);

        if (decision == 0) {
            GD.Print($"Player {PlayerNumber} hasn't acted during their priority.");
            return false;
        }
        else {
            GD.Print($"Player {PlayerNumber} has acted during their priority.");
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
            GD.Print("Could not add card " + card.CardName + " in player " + PlayerNumber + "'s hand");
        }
    }

    protected virtual void AddCardInHand(CardBase card) {
        CardsInHand.Add(card);
        card.OnAddedToPlayerHand(this);

        PrintCardsInHand();
    }

    void PrintCardsInHand() {
        GD.Print("Cards in hand of player " + PlayerNumber + ":");
        foreach (var card in CardsInHand) {
            GD.Print(" - " + card.CardName);
        }
    }


    // Common effects
    public void GainOrLoseGold(int amount) {
        Gold += amount;

        GD.Print($"Player {PlayerNumber} has {Gold} gold.");
    }
}
