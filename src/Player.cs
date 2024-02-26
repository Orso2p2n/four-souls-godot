using Godot;
using Godot.Collections;
using System;

public partial class Player : Node
{
    public Game Game { get; set; }

    public CardBase CharacterCard { get; set; }

    public int Gold { get; set; }

    public Array<CardBase> CardsInHand { get; set; } = new Array<CardBase>();

    public int PlayerNumber { get; set; }

    public PlayerLocation PlayerLocation { get; set; }

    public virtual void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        Game = game;
        PlayerNumber = playerNumber;
        PlayerLocation = playerLocation;
    }

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

    public void GetPriority() {}

    public void GainOrLoseGold(int amount) {
        Gold += amount;

        GD.Print($"Player {PlayerNumber} has {Gold} gold.");
    }
}
