using Godot;
using System;
using Godot.Collections;

public partial class DeckResource : Resource
{
    [Export] public DeckTypeResource DeckType { get; set; }
    [Export] public Array<CardResource> Cards { get; set; }
    // [Export] public Dictionary<CardResource,int> cards;
}
