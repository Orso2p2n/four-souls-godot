using Godot;
using System;

public enum DeckType {
    Character,
    Loot,
    Monster,
    Treasure,
    BonusSoul,
    Node3D
}

public partial class DeckTypeResource : Resource
{
    [Export] public string Name { get; set; }
    [Export] public DeckType deckType { get; set; }
    [Export] public Texture2D BackTexture { get; set; }
}