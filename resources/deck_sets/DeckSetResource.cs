using Godot;
using System;
using Godot.Collections;

public enum DeckSet {
    BaseGame,
    FourSoulsPlus,
    Requiem
}

public partial class DeckSetResource : Resource
{
    [Export] public string name;
    [Export] public DeckSet deckSet;
    [Export] public Array<DeckResource> decks;
}
