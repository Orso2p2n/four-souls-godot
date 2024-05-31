using Godot;
using System;

public partial class HUD : Control
{
	[Export] public Hand Hand { get; set; }
	[Export] public Label PhaseLabel { get; set; }
	[Export] public Button EndTurnButton { get; set; }
	[Export] public PriorityIntentionPanel PriorityIntentionPanel { get; set; }

    public override void _EnterTree() {
        EndTurnButton.Visible = false;
        PriorityIntentionPanel.Visible = false;
    }

    public void SetPhase(TurnPhase phase) {
		PhaseLabel.Text = phase.ToString();
	}

	public void ToggleEndTurnButton(bool active) {
		EndTurnButton.Visible = active;
	}

	public void TogglePriorityIntentionPanel(bool active) {
		PriorityIntentionPanel.Visible = active;
		// PriorityIntentionPanel.Visible = true;
	}
}
