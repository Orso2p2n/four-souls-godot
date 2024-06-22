using Godot;
using Godot.Collections;
using System;

public partial class Assets : Node
{
    [ExportGroup("Resources")]
    [ExportSubgroup("Deck Types")]
    [Export] public DeckTypeResource DeckTypeLoot { get; set; }
    [Export] public DeckTypeResource DeckTypeCharacter { get; set; }
    [Export] public DeckTypeResource DeckTypeStartingItem { get; set; }
    [Export] public DeckTypeResource DeckTypeTreasure { get; set; }
    [ExportSubgroup("Deck Sets")]
    [Export] public DeckSetResource DeckSetBaseGame { get; set; }
    
    [ExportGroup("Scenes")]
    [Export] public PackedScene CardScene { get; set; }
    [Export] public PackedScene DeckScene { get; set; }
    [Export] public PackedScene GameBoardScene { get; set; }
    [Export] public PackedScene MainPlayerScene { get; set; }
    [Export] public PackedScene OnlinePlayerScene { get; set; }
    [Export] public PackedScene CpuPlayerScene { get; set; }

    [ExportGroup("Textures")]
    [ExportSubgroup("Structure")]
    [Export] public Texture2D BotBase { get; set; }
    [Export] public Texture2D TopBase { get; set; }
    [Export] public Texture2D BotCharacter { get; set; }
    [Export] public Texture2D TopCharacter { get; set; }
    [ExportSubgroup("Stats")]
    [Export] public Texture2D StatblockCharacter { get; set; }
    [Export] public Texture2D StatblockMonster { get; set; }
    [ExportSubgroup("Addons")]
    [Export] public Texture2D Addon1Soul { get; set; }
    [Export] public Texture2D Addon2Soul { get; set; }
    [Export] public Texture2D AddonCharmed { get; set; }

    public static Assets ME { get; private set; }

    public override void _EnterTree() {
        ME = this;
    }

    public static Array<T> GetAllResourcesOfTypeInPath<[MustBeVariant] T>(string path) where T : Resource {
        var filePaths = GetAllFilesInPath(path);
        
        var resources = new Array<T>();

        foreach (var filePath in filePaths) {
            try {
                var resource = ResourceLoader.Load<T>(filePath);
                resources.Add(resource);
            }
            catch (Exception) {
                continue;
            }
        }

        return resources;
    }

    public static Array<string> GetAllFilesInPath(string path) {
        var filePaths = new Array<string>();

        var dir = DirAccess.Open(path);
        dir.ListDirBegin();

        var fileName = dir.GetNext();
        while (fileName != "") {
            var filePath = path + "/" + fileName;
            
            if (dir.CurrentIsDir()) {
                filePaths.AddRange(GetAllFilesInPath(filePath));
            }
            else {
                filePaths.Add(filePath);
            }

            fileName = dir.GetNext();
        }

        return filePaths;
    }
}