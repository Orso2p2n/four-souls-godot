using Godot;
using System;
using System.Threading.Tasks;

public partial class CardInHand : AspectRatioContainer
{
	// Signals
	[Signal] public delegate void OnClickedEventHandler(); 
	[Signal] public delegate void OnReleasedEventHandler(); 

	[Export] public TextureRect _textureRect;
	[Export] private Area2D _area2D;
	private CollisionShape2D _collisionShape2D;
	private RectangleShape2D _rectangleShape2D;

	public CardBase CardBase { get; set; }
	public Hand Hand { get; set; }

	public int PositionInHand;

	public Vector2 TargetPosition { get; private set; }
	public float TargetYBonus { get; private set; }
	public Vector2 TargetPositionOffset { get; private set; }
	public Vector2 ?TargetPositionOverride { get; private set; } = null;

	public float TargetRotation { get; private set; }
	public float ?TargetRotationOverride { get; private set; } = null;

	public float TargetHeight { get; private set; }

	public void Init(CardBase cardBase, Hand hand) {
		CardBase = cardBase;
		Hand = hand;

		cardBase.CardVisual.TextureRefreshed += UpdateSprite;

		_collisionShape2D = _area2D.GetChild(0) as CollisionShape2D;
		_rectangleShape2D = _collisionShape2D.Shape.Duplicate() as RectangleShape2D;
		_collisionShape2D.Shape = _rectangleShape2D;

		UpdateSprite(cardBase.CardVisual.GetTexture());
	}

    public void UpdateSprite(ViewportTexture texture) {
		_textureRect.Texture = texture;
	}

    public override void _Process(double delta) {
		_collisionShape2D.Position = _textureRect.Position + _textureRect.Size/2;
        _rectangleShape2D.Size = _textureRect.Size;
		PivotOffset = Size / 2;
    }

	public override void _PhysicsProcess(double delta) {
		var lerpSpeed = 0.3f;
		
		var realTargetPos = GetRealTargetPos();
		if (GlobalPosition != realTargetPos) {
			GlobalPosition = GlobalPosition.Lerp(realTargetPos, lerpSpeed);
		}

		var realTargetRot = GetRealTargetRot();
		if (RotationDegrees != realTargetRot) {
			RotationDegrees = Mathf.Lerp(RotationDegrees, realTargetRot, lerpSpeed);
		}

		if (Size.Y != TargetHeight) {
			Size = Size.Lerp(new Vector2(Size.X, TargetHeight), lerpSpeed);
		}
	}

	// --- Set Targets ---
	public void SetTargetPosition(Vector2 targetPosition, bool instant = false) {
		TargetPosition = targetPosition;

		if (instant) {
			SnapPosition();
		}
	}

	public void SetTargetYBonus(float targetYBonus, bool instant = false) {
		TargetYBonus = targetYBonus;

		if (instant) {
			SnapPosition();
		}
	}

	public void SetTargetPositionOffset(Vector2 targetPositionOffset, bool instant = false) {
		TargetPositionOffset = targetPositionOffset;

		if (instant) {
			SnapPosition();
		}
	}

	public void SetTargetPositionOverride(Vector2 ?targetPositionOverride, bool instant = false) {
		TargetPositionOverride = targetPositionOverride;

		if (instant) {
			SnapPosition();
		}
	}

	private Vector2 GetRealTargetPos() {
		if (TargetPositionOverride != null) {
			return (Vector2) TargetPositionOverride;
		}

		return TargetPosition + Vector2.Down * TargetYBonus + TargetPositionOffset;
	}

	private void SnapPosition() {
		GlobalPosition = GetRealTargetPos();
	}

	public void SetTargetRotation(float targetRotation, bool instant = false) {
		TargetRotation = targetRotation;
		
		if (instant) {
			SnapRotation();
		}
	}

	public void SetTargetRotationOverride(float ?targetRotationOverride, bool instant = false) {
		TargetRotationOverride = targetRotationOverride;
				
		if (instant) {
			SnapRotation();
		}
	}

	private float GetRealTargetRot() {
		return TargetRotationOverride != null ? (float) TargetRotationOverride : TargetRotation;
	}

	private void SnapRotation() {
		RotationDegrees = GetRealTargetRot();
	}

	public void SetTargetHeight(float targetHeight, bool instant = false) {
		TargetHeight = targetHeight;

		if (instant) {
			Size = Size with { Y = TargetHeight };
		}
	}

	// --- Interaction ---
	void Clicked() {
		Hand.CardClicked(this);
	}
	
	void OnArea2DMouseEntered() {
		Hand.CardMouseEnter(this);
	}

	void OnArea2DMouseExited() {
		Hand.CardMouseExit(this);
	}

	void OnArea2DInputEvent(Node viewport, InputEvent @event, int shapeIdx) {
		if (@event is InputEventMouseButton eventMouseButton) {
			if (eventMouseButton.ButtonIndex == MouseButton.Left) {
				if (eventMouseButton.Pressed) {
					Clicked();
				}
			}
		}
	}

    public override bool _HasPoint(Vector2 point) {
		var rect = _textureRect.GetGlobalRect();
        return rect.HasPoint(point);
    }

	public Rect2 GetLeftRect() {
		var halfSize = _textureRect.Size with { X = _textureRect.Size.X / 2 };
		var pos = GetRealTargetPos() - halfSize with { Y = 0 };
		return new Rect2(pos, halfSize);
	}

	public Rect2 GetRightRect() {
		var halfSize = _textureRect.Size with { X = _textureRect.Size.X / 2 };
		var pos = GetRealTargetPos();
		return new Rect2(pos, halfSize);
	}
}
