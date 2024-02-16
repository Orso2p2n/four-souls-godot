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
    [Export] public string name;
    [Export] public DeckType deckType;
    [Export] public Texture2D backTexture;
}