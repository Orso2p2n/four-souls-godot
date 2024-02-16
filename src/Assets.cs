using Godot;
using System;

public partial class Assets : Node
{

}

public static class StaticResources {
    public static DeckTypeResource deckTypeLoot = ResourceLoader.Load<DeckTypeResource>("res://resources/deck_types/deck_type_loot.tres");
}

public static class StaticTextures {
    public static Texture2D cardStructureBotBase = ResourceLoader.Load<Texture2D>("res://assets/sprites/card_structure/bot_base.png");
    public static Texture2D cardStructureTopBase = ResourceLoader.Load<Texture2D>("res://assets/sprites/card_structure/top_base.png");

    public static Texture2D cardStructureAddon1Soul = ResourceLoader.Load<Texture2D>("res://assets/sprites/card_structure/addon_1soul.png");
    public static Texture2D cardStructureAddon2Soul = ResourceLoader.Load<Texture2D>("res://assets/sprites/card_structure/addon_2soul.png");
    public static Texture2D cardStructureAddonCharmed = ResourceLoader.Load<Texture2D>("res://assets/sprites/card_structure/addon_charmed.png");
}
