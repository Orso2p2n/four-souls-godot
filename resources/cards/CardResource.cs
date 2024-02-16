using Godot;
using System;
using Godot.Collections;

public partial class CardResource : Resource
{
    [Export] public string CardName;
    [Export] public Script Script;

    [ExportGroup("Properties and Addons")]
    [Export] public bool charmed = false;
    [Export(PropertyHint.Range, "0,2,")] public int soulCount = 0;

    [ExportGroup("Art")]
    [Export] public Texture2D BgArt;
    [Export] public Texture2D FgArt;
    
    public DeckTypeResource deckTypeResource {
        get {
            return GetDeckTypeResource();
        }
    }

    // Back texture
    public Texture2D backTexture {
        get {
            return deckTypeResource.backTexture;
        }
    }

    // Structure
    public Texture2D topTextBoxTexture {
        get {
            return StaticTextures.cardStructureTopBase;
        }
    }

    public Texture2D botTextBoxTexture {
        get {
            return StaticTextures.cardStructureBotBase;
        }
    }

    virtual public DeckTypeResource GetDeckTypeResource() {
        return null;
    }
}
