using Godot;
using System;

public partial class Card3DSprite : Card3DVisualElement
{
	[Export] public Sprite3D Front;
	[Export] public Sprite3D Back;

	public override Vector2 Position2D {
		get {
			return Position.GetXZ() - _offset2D;
		}
		set {
			value += _offset2D;
			Position = Position.SetXZ(value.X, value.Y);
		}
	}

	public Vector2 TargetOffset2D { get; set; }

	private Vector2 _offset2D;

	public float TargetHeight;

	public float Height {
		get {
			return Position.Y;
		}
		set {
			Position = Position.SetY(value);
		}
	}

	override public void LerpPosition(Card3D card3D, float lerpSpeed) {
		base.LerpPosition(card3D, lerpSpeed);
		
		if (_offset2D != TargetOffset2D) {
			_offset2D = _offset2D.Lerp(TargetOffset2D, lerpSpeed);

			if (_offset2D.DistanceTo(TargetOffset2D) < 0.01f) {
				_offset2D = TargetOffset2D;
			}
		}
		
		if (Height != TargetHeight) {
			Height = Mathf.Lerp(Height, TargetHeight, lerpSpeed);

			if (Mathf.Abs(TargetHeight - Height) < 0.01f) {
				Height = TargetHeight;
			}
		}
	}

	public void SetFrontTexture(Texture2D texture) {
		Front.Texture = texture;
	}

	public void SetBackTexture(Texture2D texture) {
		Back.Texture = texture;
	}
}
