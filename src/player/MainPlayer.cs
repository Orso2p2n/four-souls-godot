using Godot;
using System;
using System.Collections.Generic;

public partial class MainPlayer : Player
{
    private static HUD Hud { get { return Game.ME.Hud; } }

    public override void Init(int playerNumber, PlayerLocation playerLocation) {
        base.Init(playerNumber, playerLocation);

        Hud.EndTurnButton.Pressed += EndActionPhase;
    }

    public override void StartActionPhase() {
        base.StartActionPhase();

        Hud.ToggleEndTurnButton(true);
    }

    public override void EndActionPhase() {
        base.EndActionPhase();

        Hud.ToggleEndTurnButton(false);
    }

    protected override void AddCardInHand(CardBase card) {
        base.AddCardInHand(card);

        card.TurnIntoControl(Hud.Hand);

        card.CardControl.CustomMinimumSize = new Vector2(Hud.Hand.Size.Y * 0.7f, 0);
    }
}
