using Godot;
using System;
using Godot.Collections;

public partial class MoneyLootCard : LootCard
{
	public virtual int MoneyAmount {
		get {
			return 1;
		}
	}

    public override string[] EffectText {
        get {
			return new string[]{
				$"gain {MoneyAmount}¢."
			};
		}
    }

    protected override void OnPlayedFromHand() {
		var callable = new Callable(this, MethodName.GainMoney);
		AddToStack(targets: PlayerOwnerAsArray, effectCallable: callable, effectTextIndex: 0);
    }

	private void GainMoney(Array<Player> targets) {
		GainOrLoseGold(targets, MoneyAmount);
	}
}
