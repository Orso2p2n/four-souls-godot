using Godot;

public static class NodeExtensions
{
    public static void ChangeParent(this Node node, Node parent) {
        if (node.GetParent() != null) {
            node.GetParent().RemoveChild(node);
        }

        parent.AddChild(node, forceReadableName: true);
    }

    public static void ChangeParent(this Node3D node3d, Node parent, bool keepGlobalTransform = false) {
        Transform3D oldTransform;
        if (node3d.IsInsideTree()) {
            oldTransform = node3d.GlobalTransform;
        }
        else {
            oldTransform = node3d.Transform;
        }

        ChangeParent(node3d as Node, parent);

        if (keepGlobalTransform) {
            node3d.GlobalTransform = oldTransform;
        }
    }
}