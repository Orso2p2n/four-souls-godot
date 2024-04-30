using Godot;

public static class VectorExtensions
{	
    // Vector3 to Vector2
    public static Vector2 GetXY(this Vector3 vector3)
    {
        return new Vector2(vector3.X, vector3.Y);
    }

    public static Vector2 GetYZ(this Vector3 vector3)
    {
        return new Vector2(vector3.Y, vector3.Z);
    }

    public static Vector2 GetXZ(this Vector3 vector3)
    {
        return new Vector2(vector3.X, vector3.Z);
    }

    // Set one component of Vector3
    public static Vector3 SetX(this Vector3 vector3, float X)
    {
        vector3.X = X;
        return vector3;
    }

    public static Vector3 SetY(this Vector3 vector3, float Y)
    {
        vector3.Y = Y;
        return vector3;
    }

    public static Vector3 SetZ(this Vector3 vector3, float Z)
    {
        vector3.Z = Z;
        return vector3;
    }

    // Set two components of Vector3
    public static Vector3 SetXY(this Vector3 vector3, float X, float Y)
    {
        vector3.X = X;
        vector3.Y = Y;
        return vector3;
    }

    public static Vector3 SetYZ(this Vector3 vector3, float Y, float Z)
    {
        vector3.Y = Y;
        vector3.Z = Z;
        return vector3;
    }

    public static Vector3 SetXZ(this Vector3 vector3, float X, float Z)
    {
        vector3.X = X;
        vector3.Z = Z;
        return vector3;
    }
}