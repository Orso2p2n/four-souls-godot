using Godot;
using System;
using System.Collections.Generic;



public partial class Player : Node
{
    public Game game;

    public CardBase character;

    public int gold;

    public List<CardBase> cardsInHand = new List<CardBase>();

    public int playerNumber;

    public PlayerLocation playerLocation;

    public virtual void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        this.game = game;
        this.playerNumber = playerNumber;
        this.playerLocation = playerLocation;
    }

    public virtual void AddCardInHand(CardBase card) {
        cardsInHand.Add(card);

        PrintCardsInHand();
    }

    void PrintCardsInHand() {
        GD.Print("Cards in hand of player " + playerNumber + ":");
        foreach (var card in cardsInHand) {
            GD.Print(" - " + card.cardResource.CardName);
        }
    }
}
