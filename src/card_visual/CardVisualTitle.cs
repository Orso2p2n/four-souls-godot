using Godot;
using System;
using System.Numerics;
using System.Threading.Tasks;

public partial class CardVisualTitle : RichTextLabel
{
	FontVariation font;

    public override void _Ready() {
		var oldFont = Theme.GetFont("normal_font", "RichTextLabel");
        font = oldFont.Duplicate() as FontVariation;
		AddThemeFontOverride("normal_font", font);
    }

    public async Task SetText(string text) {
		var container = GetParent() as Control;

		// Set text
		Text = text;

		// Wait for resizing
		await ToSignal(this, SignalName.Resized);

		var realSize = Size * Scale;
		var containerCenter = container.Size / 2;

		int maxSpacingGlyph = -8;

		// Rescale title on X
		while (realSize.X > container.Size.X) {
			if (font.SpacingGlyph > maxSpacingGlyph) {
				font.SpacingGlyph--;
			}
			else {
				var newScale = new Godot.Vector2(Scale.X - 0.025f, 1f);
				Scale = newScale;

				realSize = Size * Scale;

				Position = containerCenter - realSize/2;
			}

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		}
	}
}
