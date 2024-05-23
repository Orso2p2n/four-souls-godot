using Godot;
using Godot.Collections;
using System;

public partial class GameServer : Game
{
    public override void _EnterTree() {
        base._EnterTree();
    }

    protected override void CreateStackManager() {
        StackManager = new StackManager { Name = "StackManager" };
        StackManager.ChangeParent(this);
    }

    protected override void CreateTurnManager() {
        TurnManager = new TurnManagerServer { Name = "TurnManager" };
        TurnManager.ChangeParent(this);
    }
}
