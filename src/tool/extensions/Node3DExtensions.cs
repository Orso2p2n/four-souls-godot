using Godot;

public static class Node3DExtensions
{
    public static Vector3 GetRightVector(this Node3D node3D) {
        return node3D.Basis.X.Normalized();
    }

    public static Vector3 GetUpVector(this Node3D node3D) {
        return node3D.Basis.Y.Normalized();
    }

    public static Vector3 GetForwardVector(this Node3D node3D) {
        return -node3D.Basis.Z.Normalized();
    }

    public static Vector3 GetGlobalRightVector(this Node3D node3D) {
        return node3D.GlobalBasis.X.Normalized();
    }

    public static Vector3 GetGlobalUpVector(this Node3D node3D) {
        return node3D.GlobalBasis.Y.Normalized();
    }

    public static Vector3 GetGlobalForwardVector(this Node3D node3D) {
        return -node3D.GlobalBasis.Z.Normalized();
    }
}