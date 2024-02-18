using Godot;
using System;

public partial class CardBase : Node
{
	[Export] public CardVisual cardVisual;
	[Export] public CardResource defaultCardResource;

	public CardResource cardResource;

	public override void _Ready() {
		if (defaultCardResource != null) {
			cardResource = defaultCardResource;
		}
	}

	public virtual void Init() { // Called once the resource is loaded
		base._Ready();

		cardVisual.card = this;
		cardVisual.Init();
	}

	public virtual string[] GetEffectText() {
		return null;
	}

	public virtual string[] GetLoreText() {
		return null;
	}
}
