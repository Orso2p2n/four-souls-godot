using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class DescriptionContainer : VBoxContainer
{
	[Export] PackedScene effectScene;
	[Export] PackedScene loreScene;
	[Export] PackedScene lineScene;
	[Export] PackedScene lineSubScene;

	List<DescriptionEffect> texts;

	float maxHeight;

	public override void _Ready() {
		maxHeight = Size.Y;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async Task SetDescription(CardBase card) {
		texts = new List<DescriptionEffect>();

		// Process effect text
		var effectText = card.GetEffectText();
		if (effectText != null) {
			ProcessText(effectText);
		}
			
		// Process lore text
		var loreText = card.GetLoreText();
		if (loreText != null) {
			AddLine();
			ProcessText(loreText, true);
		}

		if (texts.Count > 0) {
			await ToSignal(texts.Last(), SignalName.Resized);
			
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

			await ProcessTextsRescale();
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

		texts.Add(descEffect);
	}

	private async Task ProcessTextsRescale() {
		var childrenHeight = GetChildrenHeight();

		int maxSpacingGlyph = -4;

		var i = 0;

		while (childrenHeight > maxHeight && i < 99) {
			foreach (DescriptionEffect text in texts) {
				if (text.font.SpacingGlyph > maxSpacingGlyph) {
					text.font.SpacingGlyph--;
				}
				else {
					text.ReduceFontSize(1);
				}
			}

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

			childrenHeight = GetChildrenHeight();

			i++;
		}
	}

	private float GetChildrenHeight() {
		float totalHeight = 0;
		var children = GetChildren();

		foreach (Control child in children) {
			totalHeight += child.Size.Y;
		}

		return totalHeight;
	}
}
