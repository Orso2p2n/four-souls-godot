using Godot;
using Godot.Collections;
using System;

public partial class Assets : Node
{
    [ExportGroup("Resources")]
    [Export] public DeckTypeResource DeckTypeLoot { get; set; }
    
    [ExportGroup("Scenes")]
    [Export] public PackedScene CardScene { get; set; }
    [Export] public PackedScene GameBoardScene { get; set; }
    [Export] public PackedScene MainPlayerScene { get; set; }
    [Export] public PackedScene OnlinePlayerScene { get; set; }

    [ExportGroup("Textures")]
    [Export] public Texture2D CardStructureBotBase { get; set; }
    [Export] public Texture2D CardStructureTopBase { get; set; }

    [Export] public Texture2D CardStructureAddon1Soul { get; set; }
    [Export] public Texture2D CardStructureAddon2Soul { get; set; }
    [Export] public Texture2D CardStructureAddonCharmed { get; set; }

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