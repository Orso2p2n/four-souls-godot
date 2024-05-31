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

	public int ID;

	// Hand logic
	public virtual bool CanBeInHand {
		get {
			return false;
		}
	}

	public bool CanBePlayedFromHand {
		get {
			return CanBeInHand && State == CardState.InHand && PlayerOwner.LootPlays > 0;
		}
	}

	public override void _Ready() {}

	public virtual void Init() {
		Card3d.Init(this);
		// CardControl.Init(this);
		CardVisual.Init(this);
	}

	public void Destroy() {
		Console.Log("Destroying card " + CardName);
		QueueFree();
	}

	// Appearance in-world
	public void Show3D() {
		Card3d.Visible = true;
	}

	public void Hide3D() {
		Card3d.Visible = false;
	}


	// Hand
	public virtual void OnAddedToPlayerHand(Player player) {
		PlayerOwner = player;
		PlayerOwnerAsArray = new Array<Player> { player }; 
		State = CardState.InHand;
	}

	public bool TryPlayFromHand() {
		if (CanBePlayedFromHand) {
			PlayFromHand();
			return true;
		}

		return false;
	}

	private void PlayFromHand() {
		PlayerOwner.LootPlays--;
		PlayerOwner.RemoveCardFromHand(this);
		OnPlayedFromHand();
	}

	protected virtual void OnPlayedFromHand() {}


	// Effects
	protected CardEffect<T> AddToStack<[MustBeVariant] T>(Array<T> targets, Callable effectCallable, int effectTextIndex) where T : GodotObject {
		var cardEffect = new CardEffect<T>(PlayerOwner, this, targets, effectCallable, effectTextIndex);
		cardEffect.AddToStack();
		return cardEffect;
	}

	protected void GainOrLoseGold(Array<Player> targets, int amount) {
		foreach (var target in targets) {
			target.GainOrLoseGold(amount);
		}
	}
}
