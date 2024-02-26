using Godot;
using Godot.Collections;
using System;

public partial class GameBoard : Node3D
{
	[Export] public Node PlayerLocationsContainer { get; set; }

	public Array<PlayerLocation> PlayerLocations { get; set; }

    public override void _Ready() {
		PlayerLocations = new Array<PlayerLocation>();

		foreach (var child in PlayerLocationsContainer.GetChildren()) {
			PlayerLocations.Add(child as PlayerLocation);
		}
    }
}
