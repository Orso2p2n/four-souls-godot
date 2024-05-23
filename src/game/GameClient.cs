using Godot;
using Godot.Collections;
using System;

public partial class GameClient : Game
{
    public override void _EnterTree() {
        base._EnterTree();
    }

    protected override void CreateStackManager() {
        StackManager = new StackManager { Name = "StackManager" };
        StackManager.ChangeParent(this);
    }

    protected override void CreateTurnManager() {
        TurnManager = new TurnManagerClient { Name = "TurnManager" };
        TurnManager.ChangeParent(this);
    }
}
