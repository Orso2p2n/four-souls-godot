using Godot;
using System;

[Tool]
public partial class StartingItemCardResource : CardResource
{
    public override bool IsItem {
        get {
            return true;
        }
    }

    public override DeckTypeResource GetDeckTypeResource() {
        return Assets.ME.DeckTypeStartingItem;
    }
}
