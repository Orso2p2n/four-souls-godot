using Godot;
using System;

public partial class StackEffect : GodotObject
{
    public StackEffect() {
        GD.Print("StackEffect constructor");
    }

    public virtual void Resolve() {}

    public void AddToStack() {
        OnAddedToStack();
        Game.ME.StackManager.AddEffect(this);
    }

    protected virtual void OnAddedToStack() {}
}