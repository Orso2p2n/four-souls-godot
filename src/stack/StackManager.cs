using Godot;
using Godot.Collections;
using System;

public partial class StackManager : Node
{
    public static StackManager ME;

    public Array<StackEffect> EffectsOnStack;

    private int _curPriorityPlayerNumber;
    private int _curPriorityStartNumber;

    public override void _Ready() {
        ME = this;

        Clear();
    }

    public void AddEffect(StackEffect effect) {
        EffectsOnStack.Add(effect);
    }

    public void Clear() {
        EffectsOnStack = new Array<StackEffect>();
    }

    public void Resolve() {
        var length = EffectsOnStack.Count;
        for (int i = length - 1; i >= 0 ; i--) {
            var stackEffect = EffectsOnStack[i];

            stackEffect.Resolve();
        }
    }
    
    public void StartPriority(int playerStartNumber) {
        SetPriorityStart(playerStartNumber);

        PassPriority();
    }

    public void SetPriorityStart(int playerStartNumber) {
        _curPriorityStartNumber = _curPriorityPlayerNumber = playerStartNumber;
    }

    private void PassPriority(bool first = false) {
        var player = Game.ME.Players[_curPriorityPlayerNumber];
        var playerHasActed = player.GetPriority();

        if (_curPriorityPlayerNumber == _curPriorityStartNumber && !first) {
            return;
        }

        _curPriorityPlayerNumber++;

        var maxPlayers = Game.ME.Players.Count;
        if (_curPriorityPlayerNumber >= maxPlayers) {
            _curPriorityPlayerNumber -= maxPlayers;
        }

        if (playerHasActed) {
            StartPriority(_curPriorityPlayerNumber);
        }
        else {
            PassPriority();
        }
    }
}
