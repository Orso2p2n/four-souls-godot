using Godot;
using System;

public partial class DeckLocation : Marker3D
{
	[Export] public DeckType DeckType;
	[Export] public float DeckOffset;
	[Export] public float DiscardOffset;

	public Vector3 GetDeckLocation() {
		var position = GlobalPosition;
		var right = GlobalTransform.Basis.X;
		return position + right * DeckOffset;
	}

	public Vector3 GetDiscardLocation() {
		var position = GlobalPosition;
		var right = GlobalTransform.Basis.X;
		return position + right * DiscardOffset;
	}
}
