using Godot;
using System;

public partial class CardBase : Node
{
	[Export] public CardVisual cardVisual;

	[Export] public Card3D card3d;
	[Export] public CardControl cardControl;

	public CardResource cardResource;

	public override void _Ready() {}

	public virtual void Init() {
		card3d.Init(this);
		cardControl.Init(this);
		cardVisual.Init(this);
	}

	public void TurnInto3D(Vector3? atPos = null) {
		cardControl.Visible = false;
		card3d.Visible = true;

		if (atPos != null) {
			card3d.Position = (Vector3) atPos;
		}
	}

	public void TurnIntoControl() {
		card3d.Visible = false;
		cardControl.Visible = true;
	}

	public virtual string[] GetEffectText() {
		return null;
	}

	public virtual string[] GetLoreText() {
		return null;
	}
}
