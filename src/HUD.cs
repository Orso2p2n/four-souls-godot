using Godot;
using System;

public partial class HUD : Control
{
	[Export] public Control Hand { get; set; }
	[Export] public Label PhaseLabel { get; set; }
	[Export] public Button EndTurnButton { get; set; }

    public override void _EnterTree() {
        EndTurnButton.Visible = false;
    }

    public void SetPhase(TurnPhase phase) {
		PhaseLabel.Text = phase.ToString();
	}

	public void ToggleEndTurnButton(bool active) {
		EndTurnButton.Visible = active;
	}
}
