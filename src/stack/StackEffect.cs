using Godot;
using System;

public partial class StackEffect : GodotObject
{
    public Player Owner { get; set; } 

    public StackEffect(Player owner) {
        Owner = owner;
        GD.Print("StackEffect constructor");
    }

    public virtual void Resolve() {}

    public void AddToStack() {
        OnAddedToStack();
        Game.ME.StackManager.AddEffect(this, Owner);
    }

    protected virtual void OnAddedToStack() {}
}