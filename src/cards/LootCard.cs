using Godot;
using System;

public partial class LootCard : CardBase
{
    public override bool CanBeInHand {
		get {
			return true;
		}
	}
}
