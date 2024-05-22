using Godot;
using System;
using System.Threading.Tasks;

public partial class MainPlayer : Player
{
    public static MainPlayer ME { get; private set; }

    [Export] private Node3D _origin;
    [Export] public HUD Hud { get; set; }

    public override void _EnterTree() {
        ME = this;
    }

    public override void Init(int playerNumber, PlayerLocation playerLocation) {
        base.Init(playerNumber, playerLocation);

        _origin.GlobalPosition = playerLocation.GlobalPosition;
        _origin.GlobalRotation = playerLocation.GlobalRotation;

        Hud.EndTurnButton.Pressed += EndActionPhase;

        Hud.PriorityIntentionPanel.YesPressed += PriorityIntentionYes;
        Hud.PriorityIntentionPanel.NoPressed += PriorityIntentionNo;
    }

    public override void AskForPriorityIntention() {
        base.AskForPriorityIntention();

        Hud.TogglePriorityIntentionPanel(true);
    }

    protected override void PriorityIntentionYes() {
        base.PriorityIntentionYes();

        Hud.TogglePriorityIntentionPanel(false);
    }

    protected override void PriorityIntentionNo() {
        base.PriorityIntentionNo();

        Hud.TogglePriorityIntentionPanel(false);
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
