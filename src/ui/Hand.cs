using Godot;
using Godot.Collections;
using System;

public partial class Hand : Control
{
	[Export] private PackedScene _cardInHandScene;

	public Array<CardInHand> Cards { get; set; } = new();

    private Array<CardInHand> _cardsHovered = new();
    private CardInHand _curCardHovered;
    private CardInHand _lastCardHeld;
    private CardInHand _curCardHeld;

	private float _cardHeight;
	private float _cardHeightWhenHovered;
	private float _cardWidth;
	private float _cardSeparation;

    private Vector2 _mousePos;

	public override void _Ready() {
        OnResized();
        GetViewport().PhysicsObjectPickingSort = true;
	}

    public override void _PhysicsProcess(double delta) {
        // Failsafe for when releasing the mouse while holding a card
        if (_curCardHeld == null && _lastCardHeld != null) {
            if (!_lastCardHeld._HasPoint(_mousePos)) {
                StopHoveringCard();
            }
        }
    }

    public async void OnResized() {
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        _cardHeight = Size.Y / 0.75f;
		_cardHeightWhenHovered = _cardHeight * 1.33f;

        if (Cards.Count > 0) {
            _cardWidth = _cardHeight * Cards[0].Ratio;
            _cardSeparation = _cardWidth * 0.75f;
        }

        RearrangeCards(true);
    }

	public void AddCard(CardBase card) {
		var cardInHand = _cardInHandScene.Instantiate() as CardInHand;
		cardInHand.ChangeParent(this);
		cardInHand.Init(card, this);

        cardInHand.PositionInHand = Cards.Count;
        cardInHand.ZIndex = -1;

        _cardWidth = _cardHeight * cardInHand.Ratio;
        _cardSeparation = _cardWidth * 0.75f;

		Cards.Add(cardInHand);

		RearrangeCards();
	}

    private float CalculateLerpVal(int cardsCount, int i) {
        var widthTakenByCards = _cardSeparation * cardsCount;

		var cardWidthRatio = _cardSeparation / Size.X;
        var widthRatioTakenByCards = cardWidthRatio * cardsCount;
        
        if (widthTakenByCards > Size.X + _cardSeparation) { // Too many cards
            return i / (cardsCount - 1f);
        }
        else {
            var lerpVal = i * cardWidthRatio;
            lerpVal += 0.5f - ((widthRatioTakenByCards - cardWidthRatio) / 2);
            return lerpVal;
        }
    }

	private void RearrangeCards(bool instant = false) {
		var cardsCount = Cards.Count;

		if (cardsCount == 0) {
			return;
		}
        
        float distBetweenCards = _cardSeparation;

        // Calculate X
		for (int i = 0; i < cardsCount; i++) {
			var card = Cards[i];

			float lerpVal = CalculateLerpVal(cardsCount, i);

            if (i == 1) {
                var oldLerpVal = CalculateLerpVal(cardsCount, 0);
                distBetweenCards = (lerpVal - oldLerpVal) * Size.X;
            }

            // Calc X
			var x = Mathf.Lerp(0, Size.X, lerpVal);
			card.SetTargetPosition(new Vector2(GlobalPosition.X + x, GlobalPosition.Y), instant);

            // Reset rotation
            card.SetTargetRotation(0f);

            if (card == _curCardHeld) {
                continue;
            }

            // Reset height
            card.SetTargetHeight(_cardHeight, true);
		}

        // Calculate rotation and y
        var oldY = 0f;
        var distFactor = distBetweenCards / _cardWidth;
        for (int i = Mathf.CeilToInt(cardsCount/2f); i < cardsCount; i++) {
            var card = Cards[i];
            var mirrorIdx = (cardsCount - i) - 1;
            var mirrorCard = Cards[mirrorIdx];

			float lerpVal = CalculateLerpVal(cardsCount, i);

            // Calc rotation
            var maxRot = 10f;
            var rot = Mathf.Lerp(-maxRot, maxRot, lerpVal);
            card.SetTargetRotation(rot);
            mirrorCard.SetTargetRotation(-rot);

            // Calc Y
            var angRad = Mathf.DegToRad(Mathf.Abs(rot));
            var y = (Mathf.Sin(angRad) * _cardWidth) * 0.75f * distFactor;

            card.SetTargetYBonus(y + oldY);
            mirrorCard.SetTargetYBonus(y + oldY);

            oldY += y * 2;
        }
	}

    // --- Card Hovering ---
    public void CardMouseEnter(CardInHand card) {
        if (_cardsHovered.Contains(card)) {
            return;
        }

        _cardsHovered.Add(card);

        if (_curCardHovered == null) {
            StartHoveringCard(card);
        }
    }

    public void CardMouseExit(CardInHand card) {
        if (card == _curCardHeld) {
            return;
        } 

        _cardsHovered.Remove(card);

        if (_curCardHovered == card) {
            StopHoveringCard();
            
            if (_cardsHovered.Count > 0) {
                StartHoveringCard(_cardsHovered[0]);
            }
        }
    }

    private void StartHoveringCard(CardInHand cardToHover) {
        _curCardHovered = cardToHover;

        for (int i = 0; i < Cards.Count; i++) {
			var card = Cards[i];

            if (card == cardToHover) {
                continue;
            }

            var diff = card.PositionInHand - cardToHover.PositionInHand;
            var offset = (_cardSeparation / 2) / diff;

            card.SetTargetPositionOffset(new Vector2(offset, 0));
        }

        cardToHover.SetTargetPositionOffset(Vector2.Down * (Size.Y - _cardHeightWhenHovered - cardToHover.TargetYBonus));
        cardToHover.SetTargetRotationOverride(0, true);
        cardToHover.SetTargetHeight(_cardHeightWhenHovered, true);
        cardToHover.ZIndex = 0;
    }

    private void StopHoveringCard() {
        if (_curCardHovered == null) {
            return;
        }

        for (int i = 0; i < Cards.Count; i++) {
			var card = Cards[i];

            card.SetTargetPositionOffset(Vector2.Zero);
        }

        _curCardHovered.SetTargetRotationOverride(null);
        _curCardHovered.SetTargetHeight(_cardHeight);
        _curCardHovered.ZIndex = -1;

        _curCardHovered = null;
        _lastCardHeld = null;
    }

    // --- Card clicking and moving ---
    public void CardClicked(CardInHand card) {
        if (_curCardHeld != null) {
            return;
        }

        _curCardHeld = card;
        _lastCardHeld = card;
    }

    private void ClickReleased() {
        if (_curCardHeld == null) {
            return;
        }
        
        _curCardHeld.SetTargetPositionOverride(null);
        
        _curCardHeld = null;
    }

    private void MouseMoved(Vector2 pos) {
        _mousePos = pos;

        if (_curCardHeld == null) {
            return;
        }

        var centerPos = _mousePos - new Vector2(0, _cardHeightWhenHovered / 2f);
        _curCardHeld.SetTargetPositionOverride(centerPos);

        foreach (var card in Cards) {
            if (card == _curCardHeld) {
                continue;
            }

            var checkLeftRect = true;
            var checkRightRect = true;
            
            if (card.PositionInHand == _curCardHeld.PositionInHand + 1) {
                checkLeftRect = false;
            }

            if (card.PositionInHand == _curCardHeld.PositionInHand - 1) {
                checkRightRect = false;
            }

            var leftRect = card.GetLeftRect();
            if (checkLeftRect && leftRect.HasPoint(_mousePos)) {
                InsertAtCard(card, false);
                break;
            }

            var rightRect = card.GetRightRect();
            if (checkRightRect && rightRect.HasPoint(_mousePos)) {
                InsertAtCard(card, true);
                break;
            }
        }
    }

    private void InsertAtCard(CardInHand cardToInsertAt, bool after) {
        var afterString = after ? "after" : "before";
        
        var indexToInsertAt = cardToInsertAt.PositionInHand;
        if (after) {
            indexToInsertAt++;
        }

        var indexToRemove = _curCardHeld.PositionInHand;
        if (indexToRemove > indexToInsertAt) {
            indexToRemove++;
        }

        if (indexToInsertAt > Cards.Count) {
            Cards.Add(_curCardHeld);
        }
        else {
            Cards.Insert(indexToInsertAt, _curCardHeld);
        }

        Cards.RemoveAt(indexToRemove);

        for (int i = 0; i < Cards.Count; i++) {
            var card = Cards[i];
            MoveChild(card, i);
            card.PositionInHand = i;
        }

        RearrangeCards();
        StartHoveringCard(_curCardHeld);
    }

    public override void _Input(InputEvent @event) {
        if (@event is InputEventMouseMotion eventMouseMotion) {
            MouseMoved(eventMouseMotion.Position);
        }

        else if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Left) {
                if (!eventMouseButton.Pressed) {
                    ClickReleased();
                }
            }
        }
    }
}
