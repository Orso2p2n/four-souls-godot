using Godot;
using Godot.Collections;
using System;

public partial class GameClient : Game
{
    protected override void InitDone(int peerId = -1) {
        RpcId(1, MethodName.InitDone, NetworkManager.ME.PeerID);
    }

    protected override void SetRngSeed(ulong seed) {
        base.SetRngSeed(seed);
    }

    protected override void CreateStackManager() {
        StackManager = new StackManagerClient { Name = "StackManager" };
        StackManager.ChangeParent(this);
    }

    protected override void CreateTurnManager() {
        TurnManager = new TurnManagerClient { Name = "TurnManager" };
        TurnManager.ChangeParent(this);
    }
}
