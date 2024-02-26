using Godot;
using System;
using Godot.Collections;

public partial class CardEffect<[MustBeVariant] T> : StackEffect where T : GodotObject
{
    private Array<T> _targets;
    
    private Callable _effectCallable { get; set; }

    public string EffectText { get; private set; }

    public CardEffect(Array<T> targets, Callable effectCallable, string effectText) : base() {
        _targets = targets;
        _effectCallable = effectCallable;
        EffectText = effectText;
    }

    protected override void OnAddedToStack() {
        base.OnAddedToStack();
    }

    public override void Resolve() {
        base.Resolve();

        Trigger();
    }

    protected virtual void Trigger() {
        _effectCallable.Call(_targets);
    }
}