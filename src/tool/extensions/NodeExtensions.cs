using Godot;

public static class NodeExtensions
{
    public static void ChangeParent(this Node node, Node parent) {
        if (node.GetParent() != null) {
            node.GetParent().RemoveChild(node);
        }

        parent.AddChild(node);
        node.Owner = parent;
    }
}