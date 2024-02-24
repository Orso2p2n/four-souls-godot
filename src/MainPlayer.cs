using Godot;
using System;
using System.Collections.Generic;

public partial class MainPlayer : Player
{
    public static MainPlayer ME { get; set; }

    public HUD Hud { get; set; }

    public override void Init(Game game, int playerNumber, PlayerLocation playerLocation) {
        base.Init(game, playerNumber, playerLocation);

        ME = this;

        Hud = game.Hud;
    }

    public override void AddCardInHand(CardBase card) {
        base.AddCardInHand(card);

        card.TurnIntoControl(Hud.Hand);

        card.CardControl.CustomMinimumSize = new Vector2(Hud.Hand.Size.Y * 0.7f, 0);
    }
}
