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
    [Export] public string Name { get; set; }
    [Export] public DeckSet DeckSet { get; set; }
    [Export] public Array<DeckResource> Decks { get; set; }
}
