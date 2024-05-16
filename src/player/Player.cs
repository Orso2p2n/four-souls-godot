using Godot;
using Godot.Collections;
using System;

public partial class Player : Node
{
    // Signals
    [Signal] public delegate void EndActionPhaseRequestedEventHandler();

    // Getters
    private static Game Game { get { return Game.ME; } }
    private bool IsActivePlayer { get { return Game.TurnManager.ActivePlayer == this; } }
    private bool IsInActionPhase { get { return IsActivePlayer && Game.TurnManager.CurPhase == TurnPhase.ActionPhase; } }

    public CardBase CharacterCard { get; set; }

    public int Gold { get; set; }

    public Array<CardBase> CardsInHand { get; set; } = new Array<CardBase>();

    public int PlayerNumber { get; set; }

    public PlayerLocation PlayerLocation { get; set; }

    private int _lootPlays;

    public virtual void Init(int playerNumber, PlayerLocation playerLocation) {
        PlayerNumber = playerNumber;
        PlayerLocation = playerLocation;
    }

    public bool GetPriority() {
        GD.Print($"Priority gotten by player {PlayerNumber}");
        return false;
    }

    public virtual void StartActionPhase() {
        _lootPlays += 1;
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
