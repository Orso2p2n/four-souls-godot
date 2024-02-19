using Godot;
using System;
using System.Collections.Generic;

public partial class MainPlayer : Player
{
    public HUD hud;

    public override void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        base.Init(game, playerNumber, playerLocation);

        hud = game.hud;
    }

    public override void AddCardInHand(CardBase card) {
        base.AddCardInHand(card);

        
    }
}
