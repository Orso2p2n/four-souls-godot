using Godot;
using System;
using System.Collections.Generic;

public partial class Assets : Node
{
    public static List<T> GetAllResourcesOfTypeInPath<T>(string path) where T : Resource {
        var filePaths = GetAllFilesInPath(path);
        
        var resources = new List<T>();

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

    public static List<string> GetAllFilesInPath(string path) {
        var filePaths = new List<string>();

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

    // func get_all_file_paths(path: String) -> Array[String]:  
    //     var file_paths: Array[String] = []  
    //     var dir = DirAccess.open(path)  
    //     dir.list_dir_begin()  
    //     var file_name = dir.get_next()  
    //     while file_name != "":  
    //         var file_path = path + "/" + file_name  
    //         if dir.current_is_dir():  
    //             file_paths += get_all_file_paths(file_path)  
    //         else:  
    //             file_paths.append(file_path)  
    //         file_name = dir.get_next()  
    //     return file_paths
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
