using Godot;
using Godot.Collections;
using System;

public partial class GameBoard : Node3D
{
	[Export] public Node PlayerLocationsContainer { get; set; }
	[Export] public Node DeckLocationsContainer { get; set; }

	public Array<PlayerLocation> PlayerLocations { get; set; } = new();
	public Array<DeckLocation> DeckLocations { get; set; } = new();

    public override void _EnterTree() {
		var i = 1;
		foreach (var child in PlayerLocationsContainer.GetChildren()) {
			var playerLocation = child as PlayerLocation;
			PlayerLocations.Add(playerLocation);
			playerLocation.SetNumber(i);
			
			i++;
		}

		foreach (var child in DeckLocationsContainer.GetChildren()) {
			var deckLocation = child as DeckLocation;
			DeckLocations.Add(deckLocation);
		}
    }

	public DeckLocation GetDeckLocation(DeckType deckType) {
		foreach (var deckLocation in DeckLocations) {
			if (deckLocation.DeckType == deckType) {
				return deckLocation;
			}
		}

		return null;
	}
}
