using Godot;
using System;
using Godot.Collections;

public partial class DeckResource : Resource
{
    [Export] public DeckTypeResource deckType;
    [Export] public Array<CardResource> cards;
    // [Export] public Dictionary<CardResource,int> cards;
}
