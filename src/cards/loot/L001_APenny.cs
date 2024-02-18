using Godot;
using System;

public partial class L001_APenny : CardBase
{
	public override void Init() {
		base.Init();

		GD.Print("Penny init");
	}

    public override string[] GetEffectText() {
        return new string[]{
			"gain 1¢."
		};
    }
}
