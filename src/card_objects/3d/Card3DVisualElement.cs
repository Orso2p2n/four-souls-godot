using Godot;
using System;

public partial class Card3DVisualElement : Sprite3D
{
	virtual public Vector2 Position2D {
		get {
			return Position.GetXZ();
		}
		set {
			Position = Position.SetXZ(value.X, value.Y);
		}
	}

	virtual public void LerpPosition(Card3D card3D, float lerpSpeed) {
		if (Position2D != card3D.Position2D) {
			Position2D = Position2D.Lerp(card3D.Position2D, lerpSpeed);

			if (Position2D.DistanceTo(card3D.Position2D) < 0.01f) {
				Position2D = card3D.Position2D;
			}
		}
	}
}
