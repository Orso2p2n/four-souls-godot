using Godot;
using System;
using Godot.Collections;

public enum StatType {
    None,
    Character,
    Monster
}

[Tool]
public partial class CardResource : Resource
{
    [Export]public string CardName { get; set; }
    [Export] public Script Script { get; set; }

    [ExportGroup("Properties and Addons")]
    [Export] public bool Charmed { get; set; } = false;
    [Export(PropertyHint.Range, "0,2,")] public int SoulCount { get; set; } = 0;

    [ExportGroup("Stats")]
    [Export] public StatType StatType {
        get => _statType;
        set {
            _statType = value;
            NotifyPropertyListChanged();
        }
    }
    private StatType _statType;
    [Export] public int HpStat { get; set; }
    [Export] public int DiceStat { get; set; }
    [Export] public int AtkStat { get; set; }

    [ExportGroup("Art")]
    [Export] public Texture2D BgArt { get; set; }
    [Export] public Texture2D FgArt { get; set; }
    
    public DeckTypeResource DeckTypeResource {
        get {
            return GetDeckTypeResource();
        }
    }

    // --- Back texture ---
    public Texture2D BackTexture {
        get {
            return DeckTypeResource.BackTexture;
        }
    }

    public Texture2D BackTextureCropped {
        get {
            return DeckTypeResource.BackTextureCropped;
        }
    }

    // --- Structure ---
    public virtual Texture2D TopTextBoxTexture {
        get {
            return Assets.ME.TopBase;
        }
    }

    public virtual Texture2D BotTextBoxTexture {
        get {
            return Assets.ME.BotBase;
        }
    }

    public virtual Texture2D StatblockTexture {
        get {
            switch (StatType) {
                default:
                    return null;
                
                case StatType.Character:
                    return Assets.ME.StatblockCharacter;
                
                case StatType.Monster:
                    return Assets.ME.StatblockMonster;
            }
        }
    }

    public virtual DeckTypeResource GetDeckTypeResource() {
        return null;
    }

    // --- Editor manipulation ---
    public override void _ValidateProperty(Godot.Collections.Dictionary property) {
        var name = property["name"].AsStringName();
        
        bool visible;
        if (name == PropertyName.HpStat) {
            visible = StatType != StatType.None;
        }
        else if (name == PropertyName.DiceStat) {
            visible = StatType == StatType.Monster;
        }
        else if (name == PropertyName.AtkStat) {
            visible = StatType != StatType.None;
        }
        else {
            return;
        }
        
        if (!visible) {
            var usage = PropertyUsageFlags.None;
            property["usage"] = (int) usage;
        }
    }
}
