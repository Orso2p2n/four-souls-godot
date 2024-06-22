using Godot;
using System;

[Tool]
public partial class TreasureCardResource : CardResource
{
    public override bool IsItem {
        get {
            return true;
        }
    }

    public override DeckTypeResource GetDeckTypeResource() {
        return Assets.ME.DeckTypeTreasure;
    }
}
