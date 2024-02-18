using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class DescriptionEffect : RichTextLabel
{
	[ExportGroup("Icons")]
	[Export] Texture2D hpIcon;
	[Export] Texture2D atkIcon;
	[Export] Texture2D diceIcon;

	string unprocessedText;

	int baseFontSize;
	int curFontSize;

	public override void _Ready() {
		curFontSize = baseFontSize = GetThemeFontSize("normal_font_size");
	}

	public void SetText(string text) {
		unprocessedText = text;
		
		Text = ProcessText(text);
	}

	public void RefreshText() {
		SetText(unprocessedText);
	}

	private string ProcessText(string text) {
		text = "[center]" + text;

		text = ReplaceIconsInText(text, "[HP]", hpIcon);
		text = ReplaceIconsInText(text, "[ATK]", atkIcon);
		text = ReplaceIconsInText(text, "[DICE]", diceIcon);

		return text;
	}

	private string ReplaceIconsInText(string text, string key, Texture2D icon) {
		var path = icon.ResourcePath;

		int targetHeight = curFontSize;
		int iconHeight = icon.GetHeight();
		int iconWidth = icon.GetWidth();

		float ratio = targetHeight / iconHeight;

		int targetWidth = (int) (ratio * iconWidth);
		
		string newVal = $"[img=bottom,bottom,{targetWidth}x{targetHeight}]{path}[/img]";

		return text.Replace(key, newVal);
	}
}
