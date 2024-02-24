using Godot;
using System;

public partial class CardBase : Node
{
	[Export] public CardVisual CardVisual { get; set; }

	[Export] public Card3D Card3d { get; set; }
	[Export] public CardControl CardControl { get; set; }

	public CardResource CardResource { get; set; }

	public override void _Ready() {}

	public virtual void Init() {
		Card3d.Init(this);
		CardControl.Init(this);
		CardVisual.Init(this);
	}

	public void TurnInto3D(Vector3? atPos = null) {
		CardControl.Visible = false;
		Card3d.Visible = true;

		if (atPos != null) {
			Card3d.Position = (Vector3) atPos;
		}
	}

	public void TurnIntoControl(Control parent = null) {
		Card3d.Visible = false;
		CardControl.Visible = true;

		if (parent != null) {
			CardControl.ChangeParent(parent);
		}
	}

	public virtual string[] GetEffectText() {
		return null;
	}

	public virtual string[] GetLoreText() {
		return null;
	}
}
