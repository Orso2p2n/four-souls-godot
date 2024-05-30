using Godot;
using System;
using System.Threading.Tasks;

public partial class MainPlayer : Player
{
    public static MainPlayer ME { get; private set; }

    [Export] public HUD Hud { get; set; }

    public override void _EnterTree() {
        ME = this;
    }

    public override void Init(int playerNumber, PlayerLocation playerLocation) {
        base.Init(playerNumber, playerLocation);

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

    protected override void OnCardAddedToHand(CardBase card) {
        base.OnCardAddedToHand(card);
        
        Hud.Hand.AddCard(card);

        // 
    }
}
