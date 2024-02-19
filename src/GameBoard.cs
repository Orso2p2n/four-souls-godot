using Godot;
using System;
using System.Collections.Generic;

public partial class GameBoard : Node3D
{
	[Export] public Node playerLocationsContainer;

	public List<PlayerLocation> playerLocations;

    public override void _Ready() {
		playerLocations = new List<PlayerLocation>();

		foreach (var child in playerLocationsContainer.GetChildren()) {
			playerLocations.Add(child as PlayerLocation);
		}
    }
}
