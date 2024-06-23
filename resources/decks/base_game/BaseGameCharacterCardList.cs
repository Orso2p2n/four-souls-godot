using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class BaseGameCharacterCardList : DeckCardList
{
    protected override List<CardEntry> CardsList {
        get {
            return new List<CardEntry>(){
                new CardEntry(path: "character/001_isaac"),
                new CardEntry(path: "character/002_maggy"),
                new CardEntry(path: "character/003_cain"),
                new CardEntry(path: "character/004_judas"),
                new CardEntry(path: "character/005_blue_baby"),
                new CardEntry(path: "character/006_eve"),
                new CardEntry(path: "character/007_samson"),
                new CardEntry(path: "character/008_lazarus"),
                new CardEntry(path: "character/009_lilith"),
                new CardEntry(path: "character/010_the_forgotten"),
                new CardEntry(path: "character/011_eden"),
            };
        }
    }
}