using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public enum PriorityIntention {
    Deciding,
    NotActing,
    Acting
}

public partial class StackManager : Node
{
    public static StackManager ME;

    public override void _Ready() {
        ME = this;
    }

    public virtual async Task StartPriority(int startPlayer) {
        await Task.CompletedTask;
    }

    public virtual void AddEffect(StackEffect effect, Player owner) {}
}
