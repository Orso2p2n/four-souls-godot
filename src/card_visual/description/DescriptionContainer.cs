using Godot;
using System;
using System.Threading.Tasks;
using System.Linq;
using Godot.Collections;

public partial class DescriptionContainer : VBoxContainer
{
	[Export] private PackedScene _effectScene;
	[Export] private PackedScene _loreScene;
	[Export] private PackedScene _lineScene;
	[Export] private PackedScene _lineSubScene;

	private Array<DescriptionEffect> _texts;

	private float _maxHeight;

	public override void _Ready() {
		_maxHeight = Size.Y;
	}

	public async Task SetDescription(CardBase card) {
		_texts = new Array<DescriptionEffect>();

		// Process effect text
		var effectText = card.EffectText;
		if (effectText != null) {
			ProcessText(effectText);
		}
			
		// Process lore text
		var loreText = card.LoreText;
		if (loreText != null) {
			AddLine();
			ProcessText(loreText, true);
		}

		if (_texts.Count > 0) {
			await ToSignal(_texts.Last(), SignalName.Resized);
			
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
		var line = sub ? _lineSubScene.Instantiate() : _lineScene.Instantiate();

		line.ChangeParent(this);
	}

	private void AddText(string text, bool lore = false) {
		var instance =  lore ? _loreScene.Instantiate() : _effectScene.Instantiate();

		instance.ChangeParent(this);

		var descEffect = instance as DescriptionEffect;

		descEffect.SetText(text);

		_texts.Add(descEffect);
	}

	private async Task ProcessTextsRescale() {
		var childrenHeight = GetChildrenHeight();

		int maxSpacingGlyph = -4;

		var i = 0;

		while (childrenHeight > _maxHeight && i < 99) {
			foreach (DescriptionEffect text in _texts) {
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
