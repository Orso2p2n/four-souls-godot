using Godot;
using System;

[Tool]
public partial class StartingItemCardResource : CardResource
{
    public override DeckTypeResource GetDeckTypeResource() {
        return Assets.ME.DeckTypeStartingItem;
    }
}
