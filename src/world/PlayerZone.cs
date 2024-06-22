using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class PlayerZone : Node3D
{
    [Export] private CollisionShape3D _collisionShape3D;
    private BoxShape3D _boxShape3D;
    
    [ExportGroup("Area Props")]
    [Export] public Vector2I AreaSize {
        get => _areaSize;
        set {
            SetAreaSize(value);
        }
    }
    private Vector2I _areaSize = Vector2I.One;

    private CardBase _character;
    private CardBase _startingItem;
    private Array<CardBase> _activeItems = new();
    private Array<CardBase> _passiveItems = new();
    private Array<CardBase> _others = new();

    public Array<CardBase> CardsInOrder { get; set; }

    private float _verticalRatio = 1.3f;

    public override void _EnterTree() {
        _boxShape3D = new BoxShape3D();
        _collisionShape3D.Shape = _boxShape3D;
        SetAreaSize(_areaSize);
    }

    public void SetAreaSize(Vector2I areaSize) {
        _areaSize = areaSize;
        
        if (_boxShape3D == null) {
            _boxShape3D = _collisionShape3D.Shape.Duplicate() as BoxShape3D;
            _collisionShape3D.Shape = _boxShape3D;
        }

        var newSize = _boxShape3D.Size;
        newSize.X = areaSize.X;
        newSize.Y = 0;
        newSize.Z = areaSize.Y * _verticalRatio;
        _boxShape3D.Size = newSize;
    }

    public void AddCard(CardBase card) {
        card.Show3D();
        card.Card3d.ChangeParent(this);

        // Add card to corresponding array
        var cardResource = card.CardResource;
        if (cardResource is CharacterCardResource) {
            _character = card;
        }
        else if (cardResource is StartingItemCardResource) {
            _startingItem = card;
        }
        else if (cardResource.IsItem) {
            if (cardResource.ItemType == ItemType.Active) {
                _activeItems.Add(card);
            }
            else {
                _passiveItems.Add(card);
            }
        }
        else {
            _others.Add(card);
        }

        BuildCardsArray();

        RearrangeCards();
    }

    private void BuildCardsArray() {
        CardsInOrder = new Array<CardBase>();

        if (_character != null) {
            CardsInOrder.Add(_character);
        }

        if (_startingItem != null) {
            CardsInOrder.Add(_startingItem);
        }

        CardsInOrder += _activeItems;
        CardsInOrder += _passiveItems;
        CardsInOrder += _others;
    }

    private void RearrangeCards() {
        var rearrangeTime = 0.25f;

        var curSlot = Vector2I.Zero;

        foreach (var card in CardsInOrder) {
            var pos = GetPosFromSlot(curSlot);

            var posTween = CreateTween();
			posTween.TweenProperty(card.Card3d, "position", pos, rearrangeTime).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);
            var rotTween = CreateTween();
            rotTween.TweenProperty(card.Card3d, "rotation", Vector3.Zero, rearrangeTime).SetEase(Tween.EaseType.InOut).SetTrans(Tween.TransitionType.Sine);

            curSlot = GetNextSlot(curSlot);
        }
    }

    private Vector2I GetNextSlot(Vector2I curSlot) {       
        var newX = curSlot.X + 1;
        var newY = curSlot.Y;

        if (newX >= AreaSize.X) {
            newY++;
            newX = 0;
        }
        
        var nextSlot = new Vector2I(newX, newY);
        
        return nextSlot;
    }

    private Vector3 GetPosFromSlot(Vector2I slot) {
        var basePos = new Vector3(-AreaSize.X / 2f, 0f, AreaSize.Y / 2f);

        basePos.X += slot.X + 0.5f;
        basePos.Z -= slot.Y * _verticalRatio + 0.5f;

        return basePos;
    }
}
