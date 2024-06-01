using Godot;
using System;
using Godot.Collections;

public partial class DeckResource : Resource
{
    [Export] public DeckTypeResource DeckType { get; set; }
    [Export] public CSharpScript DeckListScript { get; set; }
}
