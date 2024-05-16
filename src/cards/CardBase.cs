using Godot;
using Godot.Collections;
using System;

public enum CardState {
	None,
	InHand,
	InInventory,
	InShop,
	InMonsterSlot,
	InDiscard
}

public partial class CardBase : Node
{
	// Exports
	[Export] public CardVisual CardVisual { get; set; }

	[Export] public Card3D Card3d { get; set; }
	[Export] public CardControl CardControl { get; set; }

	// Resource stuff
	public CardResource CardResource { get; set; }
	
	public string CardName {
		get {
			return CardResource.CardName;
		}
	}

	// Texts
	public virtual string[] EffectText {
		get {
			return null;
		}
	}

	public virtual string[] LoreText {
		get {
			return null;
		}
	}

	// Logic stuff
	public Player PlayerOwner { get; set; }
	public Array<Player> PlayerOwnerAsArray { get; set; }
	public CardState State { get; set; }

	// Hand logic
	public virtual bool CanBeInHand {
		get {
			return false;
		}
	}

	public bool CanBePlayedFromHand {
		get {
			return CanBeInHand && State == CardState.InHand;
		}
	}

	public override void _Ready() {}

	public virtual void Init() {
		Card3d.Init(this);
		CardControl.Init(this);
		CardVisual.Init(this);
	}

	# region Appearance in-world
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
	#endregion

	public virtual void OnAddedToPlayerHand(Player player) {
		PlayerOwner = player;
		PlayerOwnerAsArray = new Array<Player> { player }; 
		State = CardState.InHand;
	}

	public void TryPlayFromHand() {
		if (CanBePlayedFromHand) {
			OnPlayedFromHand();
		}
	}

	protected virtual void OnPlayedFromHand() {}

	protected CardEffect<T> AddToStack<[MustBeVariant] T>(Array<T> targets, Callable effectCallable, int effectTextIndex) where T : GodotObject {
		var cardEffect = new CardEffect<T>(this, targets, effectCallable, effectTextIndex);
		cardEffect.AddToStack();
		return cardEffect;
	}

	public void Destroy() {
		GD.Print("Destroying card " + CardName);
		QueueFree();
	}

	#region Common targets and effects
	protected void GainOrLoseGold(Array<Player> targets, int amount) {
		foreach (var target in targets) {
			target.GainOrLoseGold(amount);
		}
	}
	#endregion
}
