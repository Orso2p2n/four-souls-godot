using Godot;
using System;
using Godot.Collections;

public partial class CardEffect<[MustBeVariant] T> : StackEffect where T : GodotObject
{
    public CardBase Card { get; set; }

    private Array<T> _targets;
    
    private Callable _effectCallable { get; set; }

    private int _effectTextIndex;
    public string EffectText { 
        get {
            return Card.EffectText[_effectTextIndex];
        }
    }

    public CardEffect(CardBase card, Array<T> targets, Callable effectCallable, int effectTextIndex) : base() {
        Card = card;
        _targets = targets;
        _effectCallable = effectCallable;
        _effectTextIndex = effectTextIndex;
    }

    public override void Resolve() {
        base.Resolve();

        Trigger();
    }

    protected virtual void Trigger() {
        _effectCallable.Call(_targets);
    }
}