using Godot;
using Godot.Collections;
using System;

public partial class GameServer : Game
{
    private Array<int> _peersThatCalledInitDone = new();

    protected override void InitDone(int peerId = -1) {
        if (peerId == -1) {
            peerId = NetworkManager.ME.PeerID;
        }

        _peersThatCalledInitDone.Add(peerId);
        foreach (var user in NetworkManager.ME.Users) {
            var id = (int) user.Id;
            
            if (!_peersThatCalledInitDone.Contains(id)) {
                return;
            }
        }

        // All peers have called InitDone
        _peersThatCalledInitDone.Clear();
        CallDeferred(MethodName.RpcStartGame);
    }

    private void RpcStartGame() {
        Rpc(MethodName.StartGame);
    }

    protected override void StartGame() {
        for (int i = 0; i < Players.Count; i++) {
            Rpc(MethodName.Loot, i, 3);
        }

        base.StartGame();
    }

    protected override void CreateRng() {
        base.CreateRng();
        Rpc(Game.MethodName.SetRngSeed, Rng.Seed);
    }

    protected override void SetRngSeed(ulong seed) {
        base.SetRngSeed(seed);
    }

    protected override void CreateStackManager() {
        StackManager = new StackManagerServer { Name = "StackManager" };
        StackManager.ChangeParent(this);
    }

    protected override void CreateTurnManager() {
        TurnManager = new TurnManagerServer { Name = "TurnManager" };
        TurnManager.ChangeParent(this);
    }
}
