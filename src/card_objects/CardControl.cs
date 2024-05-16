using Godot;
using System;
using System.Threading.Tasks;

public partial class CardControl : Control
{
	// Signals
	[Signal] public delegate void OnClickedEventHandler(); 
	[Signal] public delegate void OnReleasedEventHandler(); 

	[Export] private TextureRect _textureRect;
	[Export] private Area2D _area2D;
	private CollisionShape2D _collisionShape2D;
	private RectangleShape2D _rectangleShape2D;

	public CardBase CardBase { get; set; }

	public void Init(CardBase cardBase) {
		CardBase = cardBase;

		Visible = false;
		cardBase.CardVisual.TextureRefreshed += UpdateSprite;

		_collisionShape2D = _area2D.GetChild(0) as CollisionShape2D;
		_rectangleShape2D = _collisionShape2D.Shape.Duplicate() as RectangleShape2D;
		_collisionShape2D.Shape = _rectangleShape2D;
	}

    public void UpdateSprite(ViewportTexture texture) {
		_textureRect.Texture = texture;
	}

    public override void _Process(double delta) {
		_collisionShape2D.Position = _textureRect.Position + _textureRect.Size/2;
        _rectangleShape2D.Size = _textureRect.Size;
    }

	void Clicked() {
		EmitSignal(SignalName.OnClicked);
	}

	void Released() {
		EmitSignal(SignalName.OnReleased);
	}

	void OnArea2DInputEvent(Node viewport, InputEvent @event, int shapeIdx) {
		if (@event is InputEventMouseButton eventMouseButton) {
			if (eventMouseButton.ButtonIndex == MouseButton.Left) {
				if (eventMouseButton.Pressed) {
					Clicked();
				}
				else {
					Released();
				}
			}
		}
	}
}
