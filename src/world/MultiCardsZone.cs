using Godot;
using Godot.Collections;
using System;

public partial class MultiCardsZone : Node3D
{
	[Export] private Vector2 _distBetweenCards = new Vector2(1f, 0f);
	[Export] private float _rearrangeTime = 0.25f;

	public Array<Card3D> Cards { get; set; } = new();

	public void AddCard(CardBase card) {
		card.TurnInto3D();

		Cards.Add(card.Card3d);

		RearrangeCards();
	}

	private void RearrangeCards() {
		var cardsCount = Cards.Count;
		
		if (cardsCount == 0) {
			return;
		}

		var maxSideOffset = _distBetweenCards.X * cardsCount / 2; 

        for (int i = 0; i < Cards.Count; i++) {
            var card = Cards[i];
            float lerpVal = 0f;

            if (cardsCount == 1) {
                lerpVal = 0;
            }
            else {
                lerpVal = i / (cardsCount - 1f);
            }

            var x = Mathf.Lerp(-maxSideOffset, maxSideOffset, lerpVal);
            var z = _distBetweenCards.Y * i;

			var tween = CreateTween();
			tween.TweenProperty(card, "global_position", new Vector3(GlobalPosition.X + x, GlobalPosition.Y, GlobalPosition.Z + z), _rearrangeTime).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);
        }
	}
}
