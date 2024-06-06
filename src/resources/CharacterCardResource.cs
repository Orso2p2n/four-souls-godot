using Godot;
using System;

[Tool]
public partial class CharacterCardResource : CardResource
{
    [Export] public bool SpecialStartingItem {
        get => _specialStartingItem;
        set {
            _specialStartingItem = value;
            NotifyPropertyListChanged();
        }
    }
    private bool _specialStartingItem;

    [Export] public StartingItemCardResource StartingItem { get; set; }

    // --- Structure ---
    public override Texture2D TopTextBoxTexture {
        get {
            return Assets.ME.TopCharacter;
        }
    }

    public override Texture2D BotTextBoxTexture {
        get {
            return Assets.ME.BotCharacter;
        }
    }

    public override DeckTypeResource GetDeckTypeResource() {
        return Assets.ME.DeckTypeCharacter;
    }

    // --- Editor manipulation ---
    public override void _ValidateProperty(Godot.Collections.Dictionary property) {
        base._ValidateProperty(property);

        if (property["name"].AsStringName() == PropertyName.StartingItem && SpecialStartingItem) {
            var usage = property["usage"].As<PropertyUsageFlags>() | PropertyUsageFlags.ReadOnly;
            property["usage"] = (int) usage;
        }
    }
}
