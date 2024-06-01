using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BaseGameLootCardList : DeckCardList
{
    protected override List<CardEntry> CardsList {
        get {
            return new List<CardEntry>(){
                new CardEntry(path: "loot/001_a_penny", count: 6),
                new CardEntry(path: "loot/002_two_cents", count: 12),
                new CardEntry(path: "loot/003_three_cents", count: 15),
                new CardEntry(path: "loot/004_four_cents", count: 9),
                new CardEntry(path: "loot/005_a_nickel"),
                new CardEntry(path: "loot/006_a_dime"),
            };
        }
    }
}