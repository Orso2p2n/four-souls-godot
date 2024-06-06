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

[Tool]
public partial class LootCardResource : CardResource
{
    [Export] public LootCardType LootCardType { get; set; }

    public override DeckTypeResource GetDeckTypeResource() {
        return Assets.ME.DeckTypeLoot;
    }
}
