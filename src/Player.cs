using Godot;
using System;
using System.Collections.Generic;



public partial class Player : Node
{
    public Game game;

    public CardBase character;

    public int gold;

    public List<CardBase> cardsInHand;

    public int playerNumber;

    public PlayerLocation playerLocation;

    public virtual void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        this.game = game;
        this.playerNumber = playerNumber;
        this.playerLocation = playerLocation;
    }

    public virtual void AddCardInHand(CardBase card) {
        cardsInHand.Add(card);
    }
}
