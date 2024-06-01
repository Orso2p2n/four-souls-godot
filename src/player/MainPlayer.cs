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

        Hud.EndTurnButton.Pressed += OnEndTurnButtonPressed;

        Hud.PriorityIntentionPanel.YesPressed += () => { Rpc(MethodName.PriorityIntentionYes); };
        Hud.PriorityIntentionPanel.NoPressed += () => { Rpc(MethodName.PriorityIntentionNo); };
        
        Hud.SkipActionButton.Pressed += OnSkipActionButtonPressed;
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

    private void OnEndTurnButtonPressed() {
        Rpc(MethodName.EndActionPhase);
    }

    public override void EndActionPhase() {
        base.EndActionPhase();

        Hud.ToggleEndTurnButton(false);
    }

    protected override void OnCardAddedToHand(CardBase card) {
        base.OnCardAddedToHand(card);
        
        Hud.Hand.AddCard(card);
    }

    protected override void OnCardRemovedFromHand(CardBase card) {
        base.OnCardRemovedFromHand(card);

        Hud.Hand.RemoveCard(card);
    }

    public override void GetPriority() {
        base.GetPriority();

        Hud.ToggleSkipActionButton(true);
    }

    public override void RemovePriority() {
        base.RemovePriority();

        Hud.ToggleSkipActionButton(false);
    }

    private void OnSkipActionButtonPressed() {
        Rpc(Player.MethodName.EmitPriorityAction, false);
    }
}
