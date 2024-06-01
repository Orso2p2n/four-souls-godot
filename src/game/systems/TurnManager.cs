using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public enum TurnPhase {
	StartPhaseRechargeStep,
	StartPhaseAbilitiesTrigger,
	StartPhaseLootStep,
	ActionPhase,
	EndPhaseAbilitiesTrigger,
	EndPhaseMaxHandTrim,
	EndPhaseRoomDiscard,
	EndPhaseFinal
}

public partial class TurnManager : Node
{
	// Signals
	[Signal] public delegate void PhaseChangedEventHandler();

	// Getters
    private static Game Game { get { return Game.ME; } }
	
	// Variables
	public Player ActivePlayer { get; set; }
	public TurnPhase CurPhase { get; set; }

    public void StartTurn(Player firstPlayer) {
		ActivePlayer = firstPlayer;

		SetPhase(TurnPhase.StartPhaseRechargeStep);
	}

	protected virtual void EndTurn() {
		Console.Log("End turn");
	}

	protected virtual void SetPhase(TurnPhase phase) {
		CurPhase = phase;

		EmitSignal(SignalName.PhaseChanged);
		ProcessCurPhase();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected virtual void PhaseDone(int peerId = -1) {}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected void NextPhase() {
		var newPhase = CurPhase + 1;
		if (newPhase > TurnPhase.EndPhaseFinal) {
			EndTurn();
		}
		else {
			SetPhase(newPhase);
		}
	}

	protected virtual void ProcessCurPhase() {
		MainPlayer.ME.Hud.SetPhase(CurPhase);
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessStartPhaseRechargeStep() {
		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessStartPhaseAbilitiesTrigger() {
		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessStartPhaseLootStep() {
		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessActionPhase() {	
		await ToSignal(ActivePlayer, Player.SignalName.EndActionPhaseRequested);

		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessEndPhaseAbilitiesTrigger() {
		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessEndPhaseMaxHandTrim() {
		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessEndPhaseRoomDiscard() {
		PhaseDone();
	}

	[Rpc(mode: MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	protected async virtual void ProcessEndPhaseFinal() {
		PhaseDone();
	}

}
