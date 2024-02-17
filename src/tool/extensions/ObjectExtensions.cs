using Godot;

public static class ObjectExtensions
{
    public static T SafelySetScript<T>(this GodotObject obj, Resource resource) where T : GodotObject
    {
        var godotObjectId = obj.GetInstanceId();
        // Replaces old C# instance with a new one. Old C# instance is disposed.
        obj.SetScript(resource);
        // Get the new C# instance
        return GDExtension.InstanceFromId(godotObjectId) as T;
    }

    public static T SafelySetScript<T>(this GodotObject obj, string resource) where T : GodotObject
    {
        return SafelySetScript<T>(obj, ResourceLoader.Load(resource));
    }
}