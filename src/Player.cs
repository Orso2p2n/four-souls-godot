using Godot;
using System;
using System.Collections.Generic;



public partial class Player : Node
{
    public Game Game { get; set; }

    public CardBase CharacterCard { get; set; }

    public int Gold { get; set; }

    public List<CardBase> CardsInHand { get; set; } = new List<CardBase>();

    public int PlayerNumber { get; set; }

    public PlayerLocation PlayerLocation { get; set; }

    public virtual void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        Game = game;
        PlayerNumber = playerNumber;
        PlayerLocation = playerLocation;
    }

    public virtual void AddCardInHand(CardBase card) {
        CardsInHand.Add(card);

        PrintCardsInHand();
    }

    void PrintCardsInHand() {
        GD.Print("Cards in hand of player " + PlayerNumber + ":");
        foreach (var card in CardsInHand) {
            GD.Print(" - " + card.CardResource.CardName);
        }
    }
}
