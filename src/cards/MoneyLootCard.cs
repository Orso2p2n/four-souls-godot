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
		AddToStack(new Array<Player>() { PlayerOwner }, new Callable(this, MethodName.GainMoney), EffectText[0]);
    }

	private void GainMoney(Array<Player> targets) {
		GainOrLoseGold(targets, MoneyAmount);
	}
}
