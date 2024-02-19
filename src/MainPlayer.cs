using Godot;
using System;
using System.Collections.Generic;

public partial class MainPlayer : Player
{
    public static MainPlayer ME;

    public HUD hud;

    public override void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        base.Init(game, playerNumber, playerLocation);

        ME = this;

        hud = game.hud;
    }

    public override void AddCardInHand(CardBase card) {
        base.AddCardInHand(card);

        card.TurnIntoControl(hud.hand);

        card.cardControl.CustomMinimumSize = new Vector2(hud.hand.Size.Y * 0.7f, 0);
    }
}
