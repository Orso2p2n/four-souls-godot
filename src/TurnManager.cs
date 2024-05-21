using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public enum TurnPhase {
	StartPhase_RechargeStep,
	StartPhase_AbilitiesTrigger,
	StartPhase_LootStep,
	ActionPhase,
	EndPhase_AbilitiesTrigger,
	EndPhase_MaxHandTrim,
	EndPhase_RoomDiscard,
	EndPhase_Final
}

public partial class TurnManager : Node
{
	// Signals
	[Signal] public delegate void PhaseChangedEventHandler();

	// Getters
    private static Game Game { get { return Game.ME; } }
	
	// Variables
	public Player ActivePlayer {get; set;}
	public TurnPhase CurPhase {get; set;}

    public override void _EnterTree() {}

    public void StartTurn(Player firstPlayer) {
		ActivePlayer = firstPlayer;

		CurPhase = TurnPhase.StartPhase_RechargeStep;
		ProcessCurPhase();
	}

	private void EndTurn() {
		Console.Log("End turn");
	}

	private void SetPhase(TurnPhase phase) {
		CurPhase = phase;

		EmitSignal(SignalName.PhaseChanged);

		ProcessCurPhase();
	}

	private void NextPhase() {
		var newPhase = CurPhase + 1;
		if (newPhase > TurnPhase.EndPhase_Final) {
			EndTurn();
		}
		else {
			SetPhase(newPhase);
		}
	}

	private async void ProcessCurPhase() {
		Game.Hud.SetPhase(CurPhase);

		switch (CurPhase) {
			case TurnPhase.StartPhase_RechargeStep:
				await Process_StartPhase_RechargeStep();
				break;

			case TurnPhase.StartPhase_AbilitiesTrigger:
				await Process_StartPhase_AbilitiesTrigger();
				break;

			case TurnPhase.StartPhase_LootStep:
				await Process_StartPhase_LootStep();
				break;

			case TurnPhase.ActionPhase:
				await Process_ActionPhase();
				break;

			case TurnPhase.EndPhase_AbilitiesTrigger:
				await Process_EndPhase_AbilitiesTrigger();
				break;

			case TurnPhase.EndPhase_MaxHandTrim:
				await Process_EndPhase_MaxHandTrim();
				break;

			case TurnPhase.EndPhase_RoomDiscard:
				await Process_EndPhase_RoomDiscard();
				break;

			case TurnPhase.EndPhase_Final:
				await Process_EndPhase_Final();
				break;
		}

		NextPhase();
	}

	private async Task Process_StartPhase_RechargeStep() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, StartPhase_RechargeStep");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task Process_StartPhase_AbilitiesTrigger() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, StartPhase_AbilitiesTrigger");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task Process_StartPhase_LootStep() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, StartPhase_LootStep");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task Process_ActionPhase() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, ActionPhase");
		
		ActivePlayer.StartActionPhase();

		await ToSignal(ActivePlayer, Player.SignalName.EndActionPhaseRequested);
	}

	private async Task Process_EndPhase_AbilitiesTrigger() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, EndPhase_AbilitiesTrigger");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task Process_EndPhase_MaxHandTrim() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, EndPhase_MaxHandTrim");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task Process_EndPhase_RoomDiscard() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, EndPhase_RoomDiscard");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

	private async Task Process_EndPhase_Final() {
		Console.Log($"Player: {ActivePlayer.PlayerNumber}, EndPhase_Final");

		await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
	}

}
