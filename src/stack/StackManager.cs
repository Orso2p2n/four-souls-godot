using Godot;
using Godot.Collections;
using System;

public partial class StackManager : Node
{
    public static StackManager ME;

    public Array<StackEffect> EffectsOnStack;

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
    
    public void PassPriority(int playerStartNumber) {
        var length = Game.ME.Players.Count;

        for (int i = 0; i < length; i++) {
            var index = i + playerStartNumber;

            if (index >= length) {
                index -= length;
            }
            
            var player = Game.ME.Players[index];

            player.GetPriority();
        }
    }
}
