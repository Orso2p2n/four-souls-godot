using Godot;
using System;

public partial class DescriptionContainer : VBoxContainer
{
	[Export] PackedScene effectScene;
	[Export] PackedScene loreScene;
	[Export] PackedScene lineScene;
	[Export] PackedScene lineSubScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void SetDescription(CardBase card) {
		// Process effect text
		var effectText = card.GetEffectText();
		if (effectText != null) {
			ProcessText(effectText);
		}
			
		// Process lore text
		var loreText = card.GetLoreText();
		if (effectText != null) {
			AddLine();
			ProcessText(loreText, true);
		}
	}

	private void ProcessText(string[] text, bool lore = false) {
		foreach (var split in text) {
			if (split == "[LINE]") {
				AddLine();
				continue;
			}

			if (split == "[SUBLINE]") {
				AddLine(true);
				continue;
			}

			AddText(split, lore);
		}
	}

	private void AddLine(bool sub = false) {
		var line = sub ? lineSubScene.Instantiate() : lineScene.Instantiate();

		line.ChangeParent(this);
	}

	private void AddText(string text, bool lore = false) {
		var instance =  lore ? loreScene.Instantiate() : effectScene.Instantiate();

		instance.ChangeParent(this);

		var descEffect = instance as DescriptionEffect;

		descEffect.SetText(text);
	}
}
