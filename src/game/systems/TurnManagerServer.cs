using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class TurnManagerServer : TurnManager
{
    private Array<int> _peersThatCalledPhaseDone = new();

    protected override void PhaseDone(int peerId = -1) {
        if (peerId == -1) {
            peerId = NetworkManager.ME.PeerID;
        }

        Console.LogNetwork($"{peerId} called PhaseDone.");

        _peersThatCalledPhaseDone.Add(peerId);
        foreach (var user in NetworkManager.ME.Users) {
            var id = (int) user.Id;
            
            if (!_peersThatCalledPhaseDone.Contains(id)) {
                return;
            }
        }

        // All peers have called PhaseDone
        Console.LogNetwork($"All peers called PhaseDone!");
        _peersThatCalledPhaseDone.Clear();
        _ = Rpc(MethodName.NextPhase);
    }

    protected override void ProcessCurPhase() {
        base.ProcessCurPhase();

        switch (CurPhase) {
			case TurnPhase.StartPhaseRechargeStep:
                Rpc(MethodName.ProcessStartPhaseRechargeStep);
				break;

			case TurnPhase.StartPhaseAbilitiesTrigger:
                Rpc(MethodName.ProcessStartPhaseAbilitiesTrigger);
				break;

			case TurnPhase.StartPhaseLootStep:
                Rpc(MethodName.ProcessStartPhaseLootStep);
				break;

			case TurnPhase.ActionPhase:
                Rpc(MethodName.ProcessActionPhase);
				break;

			case TurnPhase.EndPhaseAbilitiesTrigger:
                Rpc(MethodName.ProcessEndPhaseAbilitiesTrigger);
				break;

			case TurnPhase.EndPhaseMaxHandTrim:
                Rpc(MethodName.ProcessEndPhaseMaxHandTrim);
				break;

			case TurnPhase.EndPhaseRoomDiscard:
                Rpc(MethodName.ProcessEndPhaseRoomDiscard);
				break;

			case TurnPhase.EndPhaseFinal:
                Rpc(MethodName.ProcessEndPhaseFinal);
				break;
		}
    }

    protected override void ProcessStartPhaseLootStep() {
        Game.ME.Rpc(Game.MethodName.Loot, ActivePlayer.PlayerNumber, 1);

        base.ProcessStartPhaseLootStep();
    }

    protected override void ProcessActionPhase() {
        ActivePlayer.Rpc(Player.MethodName.StartActionPhase);

        base.ProcessActionPhase();
    }
}