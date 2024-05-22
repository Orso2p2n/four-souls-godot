using Godot;
using System;
using Godot.Collections;

public partial class CardResource : Resource
{
    [Export]public string CardName { get; set; }
    [Export] public Script Script { get; set; }

    [ExportGroup("Properties and Addons")]
    [Export] public bool Charmed { get; set; } = false;
    [Export(PropertyHint.Range, "0,2,")] public int SoulCount { get; set; } = 0;

    [ExportGroup("Art")]
    [Export] public Texture2D BgArt { get; set; }
    [Export] public Texture2D FgArt { get; set; }
    
    public DeckTypeResource deckTypeResource {
        get {
            return GetDeckTypeResource();
        }
    }

    // Back texture
    public Texture2D backTexture {
        get {
            return deckTypeResource.BackTexture;
        }
    }

    // Structure
    public Texture2D topTextBoxTexture {
        get {
            return Assets.ME.CardStructureTopBase;
        }
    }

    public Texture2D botTextBoxTexture {
        get {
            return Assets.ME.CardStructureBotBase;
        }
    }

    virtual public DeckTypeResource GetDeckTypeResource() {
        return null;
    }
}
