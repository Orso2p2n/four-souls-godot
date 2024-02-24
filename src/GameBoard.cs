using Godot;
using System;
using System.Collections.Generic;

public partial class GameBoard : Node3D
{
	[Export] public Node PlayerLocationsContainer { get; set; }

	public List<PlayerLocation> PlayerLocations { get; set; }

    public override void _Ready() {
		PlayerLocations = new List<PlayerLocation>();

		foreach (var child in PlayerLocationsContainer.GetChildren()) {
			PlayerLocations.Add(child as PlayerLocation);
		}
    }
}
