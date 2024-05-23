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
        Rpc(MethodName.NextPhase);
    }
}