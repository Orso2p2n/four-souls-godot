using Godot;
using Godot.Collections;
using System;

public partial class GameBoard : Node3D
{
	[Export] public Node PlayerLocationsContainer { get; set; }

	public Array<PlayerLocation> PlayerLocations { get; set; }

    public override void _EnterTree() {
		PlayerLocations = new Array<PlayerLocation>();

		var i = 1;
		foreach (var child in PlayerLocationsContainer.GetChildren()) {
			var playerLocation = child as PlayerLocation;
			PlayerLocations.Add(playerLocation);
			playerLocation.SetNumber(i);
			
			i++;
		}
    }
}
