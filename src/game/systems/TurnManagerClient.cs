using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class TurnManagerClient : TurnManager
{
    protected override void PhaseDone(int peerId = -1) {
        RpcId(1, MethodName.PhaseDone, NetworkManager.ME.PeerID);
    }
}