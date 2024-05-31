using Godot;
using System;

public partial class StackEffect : GodotObject
{
    public Player Owner { get; set; } 

    public StackEffect(Player owner) {
        Owner = owner;
        Console.Log("StackEffect constructor");
    }

    public virtual void Resolve() {}

    public void AddToStack() {
        OnAddedToStack();
        StackManager.ME.AddEffect(this, Owner);
    }

    protected virtual void OnAddedToStack() {}
}