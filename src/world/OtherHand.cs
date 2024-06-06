using Fractural.Tasks;
using Godot;
using Godot.Collections;
using System;

public partial class CardInOtherHand : GodotObject {
	public Card3D Card3D { get; private set; }
	public bool AlreadyInHand { get; private set; }

	public Transform3D OriginalTransform  { get; private set; }
	public Vector3 OriginalUpVector  { get; private set; }
	public Vector3 OriginalForwardVector  { get; private set; }

	public CardInOtherHand(Card3D card3d) {
		Card3D = card3d;
		OriginalTransform = card3d.Transform;
		OriginalUpVector = card3d.GetUpVector();
		OriginalForwardVector = card3d.GetForwardVector();
	}

	public async void SetPositionAndRotation(Vector3 newPos, Vector3 newRotRad) {
		var posTween = Card3D.CreateTween();
		var rotTween = Card3D.CreateTween();
	
		var tweenTime = 0.1f;

		if (!AlreadyInHand) {
			tweenTime = 0.25f;
			AlreadyInHand = true;
		}

		posTween.TweenProperty(Card3D, "position", newPos, tweenTime).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quint);
		rotTween.TweenProperty(Card3D, "rotation", newRotRad, tweenTime).SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Quint);

		// await ToSignal(tween, Tween.SignalName.Finished);
	}
}

public partial class OtherHand : Node3D
{
	[Export] private float _maxSideSize = 2.5f;
	[Export] private float _distBetweenCards = 0.5f;
	[Export] private float _curveHeight = 1f;

	public Array<CardInOtherHand> Cards { get; set; } = new();

	public void AddCard(CardBase card) {
		card.Show3D();

		var card3d = card.Card3d;
		var cardInOtherHand = new CardInOtherHand(card3d);

		Cards.Add(cardInOtherHand);

		RearrangeCards();
	}

	public void RemoveCard(CardBase cardToRemove) {
		cardToRemove.Hide3D();

		foreach (var cardInHand in Cards) {
			if (cardInHand.Card3D.CardBase == cardToRemove) {
				Cards.Remove(cardInHand);
				break;
			}
		}

		RearrangeCards();
	}

	private float CalculateLerpVal(int cardsCount, int i) {
        var widthTakenByCards = _distBetweenCards * cardsCount;
		var fullVertSize = _maxSideSize * 2;

		var cardWidthRatio = _distBetweenCards / fullVertSize;
        var widthRatioTakenByCards = cardWidthRatio * cardsCount;
        
        if (widthTakenByCards > fullVertSize + _distBetweenCards) { // Too many cards
            return i / (cardsCount - 1f);
        }
        else {
            var lerpVal = i * cardWidthRatio;
            lerpVal += 0.5f - ((widthRatioTakenByCards - cardWidthRatio) / 2);
            return lerpVal;
        }
    }

	private void RearrangeCards() {
		var cardsCount = Cards.Count;
		
		if (cardsCount == 0) {
			return;
		}

		var left = -this.GetGlobalRightVector();
		var up = this.GetGlobalUpVector();
		var back = -this.GetGlobalForwardVector();

		for (int i = 0; i < Cards.Count; i++) {
            var card = Cards[i];
			// Calculate position
            float lerpVal = CalculateLerpVal(cardsCount, i);

			var x = Mathf.Lerp(-_maxSideSize, _maxSideSize, lerpVal);
			var y = getY(x);
			var z = Mathf.Lerp(-0.1f, 0.1f, lerpVal);

			var newPos = GlobalPosition + left * x + up * y + back * z;

			// Calculate rotation
			// Align up card with backward
			var cardUp = card.OriginalUpVector;
			var dotProduct1 = back.Dot(cardUp);
			var rot = Mathf.Acos(dotProduct1);
			var axis = back.Cross(cardUp).Normalized();

			// Rotate depending on normal
			var normal = getNormal(x);
			var vector3Normal = new Vector3(normal.X, normal.Y, 0);
			vector3Normal = vector3Normal.Rotated(up, GlobalRotation.Y - Mathf.Pi);
			var normalAngle = -normal.AngleTo(Vector2.Down);

			// Apply rotation
			var newTransform = card.OriginalTransform.Rotated(up, GlobalRotation.Y - Mathf.Pi);
			newTransform = newTransform.Rotated(axis, rot);
			newTransform = newTransform.RotatedLocal(back, normalAngle);
			var newRotRad = newTransform.Basis.GetEuler();

			card.SetPositionAndRotation(newPos, newRotRad);
		}
	}

	private float getY(float x) {
		var xSqr = Mathf.Pow(x, 2);
		var sideSqr = Mathf.Pow(_maxSideSize, 2);
		var y = _curveHeight - _curveHeight * xSqr / sideSqr;

		return y;
	}

	private Vector2 getNormal(float x) {
		var y = getY(x);

		var sideSqr = Mathf.Pow(_maxSideSize, 2);
		var derivativeY = -(2 * _curveHeight * x / sideSqr);

		var m = -(1 / derivativeY);

		// if (float.IsInfinity(m)) {
		// 	m = 999999f;
		// }

		float getNormalYAtX(float normalX) {
			var value =  m * (normalX - x) + y;

			if (float.IsNaN(value)) {
				return 0;
			}

			if (float.IsInfinity(value)) {
				return 999999;
			}

			return value;
		}

		var yAt0 = getNormalYAtX(0);
		var yAt1 = getNormalYAtX(1);

		var pos0 = new Vector2(0, yAt0);
		var pos1 = new Vector2(1, yAt1);

		Vector2 vector;

		if (yAt1 < yAt0) {
			vector = pos0 - pos1;
		}
		else {
			vector = pos1 - pos0;
		}

		return vector.Normalized();
	}
}
