using Godot;
using System;

public enum LootCardType {
    Misc,
    Trinket,
    Pill,
    Rune,
    ButterBean,
    Bomb,
    Battery,
    DiceShard,
    SoulHeart,
    LostSoul,
    Nickel,
    FourCents,
    ThreeCents,
    TwoCents,
    OneCent
}

public partial class LootCardResource : CardResource
{
    [Export] public LootCardType LootCardType { get; set; }

    override public DeckTypeResource GetDeckTypeResource() {
        return StaticResources.DeckTypeLoot;
    }
}
